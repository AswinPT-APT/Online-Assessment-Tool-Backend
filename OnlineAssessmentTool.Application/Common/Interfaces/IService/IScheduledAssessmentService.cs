using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Application.Dtos.User;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IService
{
    public interface IScheduledAssessmentService
    {
        Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId);
        Task<ScheduledAssessmentDetailsDTO> GetScheduledAssessmentDetailsAsync(int scheduledAssessmentId);
    }
}

