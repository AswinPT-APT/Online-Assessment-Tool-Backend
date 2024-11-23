using OnlineAssessmentTool.Dtos;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IIlpRepository
    {

        Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId);

        Task<IlpIntegrationScheduledAssessmentDTO> GetScheduledAssessmentDetails(string batchname);

    }
}
