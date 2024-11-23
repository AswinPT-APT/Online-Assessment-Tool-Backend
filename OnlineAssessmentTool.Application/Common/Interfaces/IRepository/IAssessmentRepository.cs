using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        Task<Assessment> GetAssessmentByIdAsync(int id);
        Task DeleteAssessmentAsync(int assessmentId);
        Task<IEnumerable<AssessmentOverviewDTO>> GetAllAssessmentOverviewsAsync();
        Task<IEnumerable<TraineeScoreDTO>> GetHighPerformersByAssessmentIdAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeScoreDTO>> GetLowPerformersByAssessmentIdAsync(int scheduledAssessmentId);
        Task<List<AssessmentTableDTO>> GetAssessmentsForTrainer(int trainerId);
        Task<List<TraineeAssessmentTableDTO>> GetTraineeAssessmentDetails(int scheduledAssessmentId);
    }
}
