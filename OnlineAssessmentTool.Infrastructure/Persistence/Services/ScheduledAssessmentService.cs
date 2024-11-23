using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Application.Dtos.User;

namespace OnlineAssessmentTool.Services
{
    public class ScheduledAssessmentService : IScheduledAssessmentService
    {
        private readonly IScheduledAssessmentRepository _scheduledAssessmentRepository;

        public ScheduledAssessmentService(IScheduledAssessmentRepository scheduledAssessmentRepository)
        {
            _scheduledAssessmentRepository = scheduledAssessmentRepository;
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetAttendedStudentsAsync(scheduledAssessmentId);
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetAbsentStudentsAsync(scheduledAssessmentId);
        }
        public async Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetTraineeAnswerDetailsAsync(traineeId, scheduledAssessmentId);
        }
        public async Task<ScheduledAssessmentDetailsDTO> GetScheduledAssessmentDetailsAsync(int scheduledAssessmentId)
        {
            return await _scheduledAssessmentRepository.GetScheduledAssessmentDetailsAsync(scheduledAssessmentId);
        }
    }
}
