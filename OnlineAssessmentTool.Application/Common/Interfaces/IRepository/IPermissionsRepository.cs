using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IPermissionsRepository : IRepository<Permission>
    {
        Task<bool> ExistsAsync(int id);
        Task<List<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames);
        Task<List<Permission>> GetByIdsAsync(List<int> permissionIds);
    }
}
