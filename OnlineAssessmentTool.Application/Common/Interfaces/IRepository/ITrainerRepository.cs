using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface ITrainerRepository : IRepository<Trainer>
    {
        public Task<Trainer> GetByUserIdAsync(int userId);
        public Task<List<string>> GetAllTrainersAsync();

    }
}
