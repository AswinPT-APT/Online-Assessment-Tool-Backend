using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;

public interface IAssessmentService
{
    Task<Assessment> GetAssessmentByIdAsync(int assessmentId);
    Task<Assessment> CreateAssessmentAsync(AssessmentDTO assessmentDTO);
    Task DeleteAssessmentAsync(int assessmentId);
}
