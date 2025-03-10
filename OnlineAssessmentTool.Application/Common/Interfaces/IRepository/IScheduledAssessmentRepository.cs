﻿using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Application.Dtos.User;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IRepository
{
    public interface IScheduledAssessmentRepository : IRepository<ScheduledAssessment>
    {
        Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId);
        Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId);
        Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId);
        Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId);
        Task<AssessmentTableDTO> GetAssessmentTableByScheduledAssessmentId(int scheduledAssessmentId);
        Task<ScheduledAssessmentDetailsDTO> GetScheduledAssessmentDetailsAsync(int scheduledAssessmentId);

    }
}
