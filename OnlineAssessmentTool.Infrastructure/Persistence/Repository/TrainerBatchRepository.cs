using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class TrainerBatchRepository : Repository<TrainerBatch>, ITrainerBatchRepository
    {
        public TrainerBatchRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<TrainerBatch>> GetByTrainerIdAsync(int trainerId)
        {
            return await _context.TrainerBatches
                .Where(tb => tb.Trainer_id == trainerId)
                .ToListAsync();
        }
        public async Task RemoveRangeAsync(IEnumerable<TrainerBatch> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
