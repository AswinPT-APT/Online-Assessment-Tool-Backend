using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {

        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}

