using OnlineAssessmentTool.Application.Dtos.Assessment;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IService
{
    public interface IAssessmentScoreService
    {
        Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs);
    }
}
