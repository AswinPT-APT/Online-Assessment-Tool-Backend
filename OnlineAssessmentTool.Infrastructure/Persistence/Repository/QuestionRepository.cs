using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {


        public QuestionRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions.Include(q => q.QuestionOptions)
                                           .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _context.Questions.Include(q => q.QuestionOptions)
                                                   .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}
