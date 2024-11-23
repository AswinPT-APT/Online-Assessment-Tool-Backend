using OnlineAssessmentTool.Application.Common.Interfaces;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Repository
{
    public class QuestionOptionRepository : Repository<QuestionOption>, IQuestionOptionRepository
    {
        public QuestionOptionRepository(IApplicationDbContext context) : base(context)
        {

        }
    }
}
