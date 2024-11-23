using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class PermissionsRepository : Repository<Permission>, IPermissionsRepository
    {
        public PermissionsRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Permissions.AnyAsync(p => p.Id == id);
        }
        public async Task<List<Permission>> GetPermissionsByNamesAsync(List<string> permissionNames)
        {
            return await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();
        }
        public async Task<List<Permission>> GetByIdsAsync(List<int> permissionIds)
        {
            return await _context.Permissions.Where(p => permissionIds.Contains(p.Id)).ToListAsync();
        }
    }
}
