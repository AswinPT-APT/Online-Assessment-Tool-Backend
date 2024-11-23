using Microsoft.EntityFrameworkCore.Storage;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Users> GetByIdAsync(int id);
    }
}
