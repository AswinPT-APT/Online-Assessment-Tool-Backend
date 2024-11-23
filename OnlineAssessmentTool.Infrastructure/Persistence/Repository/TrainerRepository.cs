using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<Trainer> GetByUserIdAsync(int userId)
        {
            return await _context.Trainers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }
        public async Task<List<string>> GetAllTrainersAsync()
        {
            return await _context.Trainers
                .Include(t => t.User)
                .Select(t => t.User.Username)
                .ToListAsync();
        }
    }
}
