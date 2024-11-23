using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Services
{
    public class AssessmentPostService : IAssessmentPostService
    {
        private readonly ITraineeAnswerRepository _traineeAnswerRepository;
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;
        private readonly ApplicationDbContext _dbContext;

        public AssessmentPostService(ITraineeAnswerRepository traineeAnswerRepository, IAssessmentScoreRepository assessmentScoreRepository)
        {
            _traineeAnswerRepository = traineeAnswerRepository;
            _assessmentScoreRepository = assessmentScoreRepository;
        }

        public async Task<List<TraineeAnswer>> ProcessTraineeAnswers(List<PostAssessmentDTO> postAssessment, int userId)
        {
            var traineeAnswers = new List<TraineeAnswer>();
            var totalScore = 0;

            foreach (var question in postAssessment)
            {
                var questionOptions = question.QuestionOptions.FirstOrDefault();

                if (questionOptions != null)
                {
                    bool isCorrect = false;

                    if (questionOptions.CorrectAnswers.Count > 1 || question.Answered.Contains(","))
                    {// Check if the question is MSQ

                        var normalizedAnsweredList = question.Answered.Split(',')
                                               .Select(a => a.Trim().ToLower())
                                               .ToList();
                        var normalizedCorrectAnswers = questionOptions.CorrectAnswers
                            .Select(answer => answer.Replace(" ", "").ToLower())
                            .ToList();

                        isCorrect = !normalizedCorrectAnswers.Except(normalizedAnsweredList).Any() &&
                                    !normalizedAnsweredList.Except(normalizedCorrectAnswers).Any();
                    }
                    else // Handle MSQ
                    {
                        var normalizedAnswered = question.Answered.Replace(" ", "").ToLower();
                        var normalizedCorrectAnswers = questionOptions.CorrectAnswers
                            .Select(answer => answer.Replace(" ", "").ToLower())
                            .ToList();


                        isCorrect = normalizedCorrectAnswers.Contains(normalizedAnswered);
                    }

                    var traineeAnswer = new TraineeAnswer
                    {
                        ScheduledAssessmentId = question.AssessmentId,
                        TraineeId = userId,
                        QuestionId = question.QuestionId,
                        Answer = question.Answered,
                        IsCorrect = isCorrect,
                        Score = isCorrect ? question.Points : 0
                    };

                    totalScore += traineeAnswer.Score;
                    traineeAnswers.Add(traineeAnswer);
                    await _traineeAnswerRepository.AddAsync(traineeAnswer);
                }
            }

            var assessmentScore = new AssessmentScore
            {
                AvergeScore = totalScore,
                ScheduledAssessmentId = postAssessment.First().AssessmentId,
                TraineeId = userId,
                CalculatedOn = DateTime.UtcNow
            };
            await _assessmentScoreRepository.AddAsync(assessmentScore);
            await _dbContext.SaveChangesAsync();

            await _dbContext.SaveChangesAsync();
            return traineeAnswers;
        }

        /*public async Task<List<TraineeAnswer>> ProcessTraineeAnswers(List<PostAssessmentDTO> postAssessment, int userId)
        {
            var traineeAnswers = new List<TraineeAnswer>();
            var totalScore = 0;

            foreach (var question in postAssessment)
            {
                var questionOptions = question.QuestionOptions.FirstOrDefault();

                if (questionOptions != null)
                {

                    var normalizedAnswered = question.Answered.Replace(" ", "").ToLower();
                    var normalizedCorrectAnswers = questionOptions.CorrectAnswers
                        .Select(answer => answer.Replace(" ", "").ToLower())
                        .ToList();


                    bool isCorrect = normalizedCorrectAnswers.Contains(normalizedAnswered);
                    var traineeAnswer = new TraineeAnswer
                    {
                        ScheduledAssessmentId = question.AssessmentId,
                        TraineeId = userId,
                        QuestionId = question.QuestionId,
                        Answer = question.Answered,
                        IsCorrect = isCorrect,
                        Score = isCorrect ? question.Points : 0
                    };

                    totalScore += traineeAnswer.Score;
                    traineeAnswers.Add(traineeAnswer);
                    await _traineeAnswerRepository.AddAsync(traineeAnswer);
                }
            }

            var assessmentScore = new AssessmentScore
            {
                AvergeScore = totalScore,
                ScheduledAssessmentId = postAssessment.First().AssessmentId,
                TraineeId = userId,
                CalculatedOn = DateTime.UtcNow
            };
            await _assessmentScoreRepository.AddAsync(assessmentScore);
            await _assessmentScoreRepository.SaveAsync();

            await _traineeAnswerRepository.SaveAsync();
            return traineeAnswers;
        }*/
    }
}
