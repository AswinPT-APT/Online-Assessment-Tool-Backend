using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface ITraineeRepository : IRepository<Trainee>
    {
        public Task<Trainee> GetByUserIdAsync(int userId);
    }
}
