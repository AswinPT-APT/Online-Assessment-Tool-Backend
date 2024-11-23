using Microsoft.EntityFrameworkCore.Storage;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<Users> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }

}

