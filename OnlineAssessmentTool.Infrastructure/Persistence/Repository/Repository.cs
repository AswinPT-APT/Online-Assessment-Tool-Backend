using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;

namespace OnlineAssessmentTool.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(IApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
