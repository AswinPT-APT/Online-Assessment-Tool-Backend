using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IBatchRepository : IRepository<Batch>
    {
        Task<bool> ExistsAsync(int id);

    }
}
