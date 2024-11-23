using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        bool RoleExists(int id);
        public Task<Role> GetByIdAsync(int id);
    }
}
