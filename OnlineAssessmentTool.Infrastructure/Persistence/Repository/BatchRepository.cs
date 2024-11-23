using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {
        public BatchRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.batch.AnyAsync(b => b.batchid == id);
        }
    }
}