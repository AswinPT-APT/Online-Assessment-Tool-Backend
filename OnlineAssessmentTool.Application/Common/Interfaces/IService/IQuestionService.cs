using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;

public interface IQuestionService
{
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<Question> AddQuestionToAssessmentAsync(int assessmentId, QuestionDTO questionDTO);
    Task<Question> UpdateQuestionAsync(int questionId, QuestionDTO questionDTO);
    Task DeleteQuestionAsync(int questionId);
}
