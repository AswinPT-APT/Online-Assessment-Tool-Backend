﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Application.Dtos.User;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Dtos;
using OnlineAssessmentTool.Persistence;
using OnlineAssessmentTool.Shared.Enums;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITrainerBatchRepository _trainerBatchRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

    public UserService(
        IUserRepository userRepository,
        ITrainerRepository trainerRepository,
        ITraineeRepository traineeRepository,
        ITrainerBatchRepository trainerBatchRepository,
        IMapper mapper,
        ApplicationDbContext context)
    {
        _userRepository = userRepository;
        _trainerRepository = trainerRepository;
        _traineeRepository = traineeRepository;
        _trainerBatchRepository = trainerBatchRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> CreateUserAsync(CreateUserDTO createUserDto, TrainerDTO trainerDto = null, TraineeDTO traineeDto = null, List<int> batchIds = null)
    {
        using (var transaction = await _userRepository.BeginTransactionAsync())
        {
            try
            {
                var user = _mapper.Map<Users>(createUserDto);
                await _userRepository.AddAsync(user);
                await _context.SaveChangesAsync();

                if (user.UserType == UserType.Trainer && trainerDto != null)
                {
                    var trainer = _mapper.Map<Trainer>(trainerDto);
                    trainer.UserId = user.UserId;
                    trainer.Password = _passwordHasher.HashPassword(null, trainerDto.Password);
                    trainer.IsActive = false;
                    trainer.LastPasswordReset = DateTime.UtcNow;

                    await _trainerRepository.AddAsync(trainer);
                    await _context.SaveChangesAsync();

                    if (batchIds != null && batchIds.Any())
                    {
                        foreach (var batchId in batchIds)
                        {
                            var trainerBatch = new TrainerBatch
                            {
                                Trainer_id = trainer.TrainerId,
                                Batch_id = batchId
                            };
                            await _trainerBatchRepository.AddAsync(trainerBatch);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                else if (user.UserType == UserType.Trainee && traineeDto != null)
                {
                    var trainee = _mapper.Map<Trainee>(traineeDto);
                    trainee.UserId = user.UserId;
                    trainee.Password = _passwordHasher.HashPassword(null, traineeDto.Password);
                    trainee.IsActive = false;
                    trainee.LastPasswordReset = DateTime.UtcNow;

                    await _traineeRepository.AddAsync(trainee);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    public async Task<List<Users>> GetUsersByRoleNameAsync(string roleName)
    {
        var lowerRoleName = roleName.ToLower();

        var query = from user in _context.Users
                    join trainer in _context.Trainers on user.UserId equals trainer.UserId
                    join role in _context.Roles on trainer.RoleId equals role.Id
                    where role.RoleName.ToLower() == lowerRoleName
                    select user;

        return await query.ToListAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userType = user.UserType;

        if (userType == UserType.Trainer)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (trainer != null)
            {
                var trainerBatches = _context.TrainerBatches.Where(tb => tb.Trainer_id == trainer.TrainerId);
                _context.TrainerBatches.RemoveRange(trainerBatches);
                _context.Trainers.Remove(trainer);
            }
        }

        if (userType == UserType.Trainee)
        {
            var trainee = await _context.Trainees.FirstOrDefaultAsync(t => t.UserId == userId);
            if (trainee != null)
            {
                _context.Trainees.Remove(trainee);
            }
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserDetailsDTO> GetUserDetailsByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var userDetails = new UserDetailsDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            UserType = user.UserType,
            IsAdmin = user.IsAdmin
        };

        if (user.UserType == UserType.Trainer)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Role)
                .ThenInclude(r => r.Permissions)
                .Include(t => t.TrainerBatch)
                .ThenInclude(tb => tb.Batch)
                .FirstOrDefaultAsync(t => t.UserId == user.UserId);

            userDetails.Trainer = trainer;
            userDetails.Role = trainer?.Role;
            userDetails.Permissions = trainer?.Role?.Permissions.ToList();
        }
        else if (user.UserType == UserType.Trainee)
        {
            var trainee = await _context.Trainees
                .Include(t => t.Batch)
                .FirstOrDefaultAsync(t => t.UserId == user.UserId);

            userDetails.Trainee = trainee;
        }

        return userDetails;
    }

    public async Task<UserDetailsDTO> GetUserEmailByUsernameAsync(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return null;

        var userDetails = new UserDetailsDTO
        {
            Email = user.Email
        };

        return userDetails;
    }

    public async Task<bool> UpdateUserAsync(
    CreateUserDTO createUserDto,
    TrainerDTO trainerDto = null,
    TraineeDTO traineeDto = null,
    List<int> batchIds = null
)
    {
        using (var transaction = await _userRepository.BeginTransactionAsync())
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(createUserDto.userId);
                if (user == null)
                {
                    return false;
                }

                user.Username = createUserDto.Username;
                user.Email = createUserDto.Email;
                user.Phone = createUserDto.Phone;
                user.IsAdmin = createUserDto.IsAdmin;

                await _userRepository.UpdateAsync(user);
                await _context.SaveChangesAsync();

                if (createUserDto.UserType == UserType.Trainer && trainerDto != null)
                {
                    var trainer = await _trainerRepository.GetByUserIdAsync(user.UserId);
                    if (trainer != null)
                    {
                        trainer.JoinedOn = trainerDto.JoinedOn;
                        trainer.Password = trainerDto.Password;
                        trainer.RoleId = trainerDto.RoleId;

                        await _trainerRepository.UpdateAsync(trainer);
                        await _context.SaveChangesAsync();

                        var existingBatches = await _trainerBatchRepository.GetByTrainerIdAsync(trainer.TrainerId);
                        await _trainerBatchRepository.RemoveRangeAsync(existingBatches);

                        if (batchIds != null && batchIds.Any())
                        {
                            foreach (var batchId in batchIds)
                            {
                                var trainerBatch = new TrainerBatch
                                {
                                    Trainer_id = trainer.TrainerId,
                                    Batch_id = batchId
                                };

                                await _trainerBatchRepository.AddAsync(trainerBatch);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (createUserDto.UserType == UserType.Trainee && traineeDto != null)
                {
                    var trainee = await _traineeRepository.GetByUserIdAsync(user.UserId);
                    if (trainee != null)
                    {
                        trainee.JoinedOn = traineeDto.JoinedOn;
                        trainee.Password = traineeDto.Password;
                        trainee.BatchId = traineeDto.BatchId;

                        await _traineeRepository.UpdateAsync(trainee);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        try
        {
            var user = await GetUserDetailsByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var userPassword = user.UserType == UserType.Trainee ? user.Trainee.Password : user.Trainer.Password;
            bool isValid = ValidatePassword(userPassword, password);
            return isValid;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> IsUserActive(string email)
    {
        try
        {
            var user = await GetUserDetailsByEmailAsync(email);
            if (user.UserType == UserType.Trainee && user.Trainee.IsActive == false)
            {
                return false;
            }
            else if (user.UserType == UserType.Trainer && user.Trainer.IsActive == false)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public bool ValidatePassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }

    public async Task<ApiResponse> UpdateUserPasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO)
    {
        var apiResponse = new ApiResponse();
        using (var transaction = await _userRepository.BeginTransactionAsync())
        {
            try
            {
                var user = await GetUserDetailsByEmailAsync(updateUserPasswordDTO.Email);
                if (user == null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    apiResponse.Result = "User with given mail id not found";
                    return apiResponse;
                }

                else if (user.UserType == UserType.Trainer && updateUserPasswordDTO != null)
                {
                    var trainer = await _trainerRepository.GetByUserIdAsync(user.UserId);
                    if (trainer != null)
                    {
                        trainer.Password = _passwordHasher.HashPassword(null, updateUserPasswordDTO.Password);
                        if (trainer.IsActive == false)
                        {
                            trainer.IsActive = true;
                        }
                        trainer.LastPasswordReset = DateTime.UtcNow;

                        await _trainerRepository.UpdateAsync(trainer);
                        await _context.SaveChangesAsync();
                    }
                }

                else if (user.UserType == UserType.Trainee && updateUserPasswordDTO != null)
                {
                    var trainee = await _traineeRepository.GetByUserIdAsync(user.UserId);
                    if (trainee != null)
                    {
                        trainee.Password = _passwordHasher.HashPassword(null, updateUserPasswordDTO.Password);
                        if (trainee.IsActive == false)
                        {
                            trainee.IsActive = true;
                        }
                        trainee.LastPasswordReset = DateTime.UtcNow;

                        await _traineeRepository.UpdateAsync(trainee);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                apiResponse.IsSuccess = true;
                apiResponse.StatusCode = System.Net.HttpStatusCode.Accepted;
                apiResponse.Result = "Password updated successfully";
                return apiResponse;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}


