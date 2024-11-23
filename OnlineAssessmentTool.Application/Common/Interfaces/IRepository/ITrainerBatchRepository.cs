using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface ITrainerBatchRepository : IRepository<TrainerBatch>
    {
        public Task<IEnumerable<TrainerBatch>> GetByTrainerIdAsync(int trainerId);
        Task RemoveRangeAsync(IEnumerable<TrainerBatch> entities);
    }
}
