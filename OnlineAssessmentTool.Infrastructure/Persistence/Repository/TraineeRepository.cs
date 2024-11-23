using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class TraineeRepository : Repository<Trainee>, ITraineeRepository
    {
        public TraineeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Trainee> GetByUserIdAsync(int userId)
        {
            return await _context.Trainees
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

    }
}
