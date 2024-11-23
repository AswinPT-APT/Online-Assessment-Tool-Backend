using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IService
{
    public interface IAssessmentPostService
    {
        Task<List<TraineeAnswer>> ProcessTraineeAnswers(List<PostAssessmentDTO> postAssessment, int userId);

    }
}
