using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IAssessmentScoreRepository : IRepository<AssessmentScore>
    {
        Task<ICollection<AssessmentScoreGraphDTO>> GetAssessmentByIdAsync(int id);
        Task<List<TraineeAssessmentScoreDTO>> GetAssessmentScoresByTraineeIdAsync(int traineeId);
        Task<AssessmentScore> GetByScheduledAssessmentAndTraineeAsync(int scheduledAssessmentId, int traineeId);
        Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs);
        Task<IEnumerable<object>> GetScoreDistributionAsync(int assessmentId);
        public Task<ActionResult<IEnumerable<TraineeAverageScoreDto>>> GetTraineesWithAverageScore(string batchName);

    }
}
