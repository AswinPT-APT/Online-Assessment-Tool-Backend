using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class TraineeAnswerRepository : Repository<TraineeAnswer>, ITraineeAnswerRepository
    {
        public TraineeAnswerRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<TraineeAnswer> GetTraineeAnswerAsync(int scheduledAssessmentId, int traineeId, int questionId)
        {
            return await _context.TraineeAnswers
                .FirstOrDefaultAsync(ta => ta.ScheduledAssessmentId == scheduledAssessmentId &&
                                            ta.TraineeId == traineeId &&
                                            ta.QuestionId == questionId);
        }

        public async Task UpdateTraineeAnswerAsync(TraineeAnswer traineeAnswer)
        {
            _context.TraineeAnswers.Update(traineeAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckTraineeAnswerExistsAsync(int scheduledAssessmentId, int userId)
        {
            return await _context.TraineeAnswers
                .AnyAsync(ta => ta.ScheduledAssessmentId == scheduledAssessmentId && ta.TraineeId == userId);
        }
    }
}
