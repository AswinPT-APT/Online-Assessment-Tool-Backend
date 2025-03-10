﻿using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Dtos;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class IlpIntegrationRepository : IIlpRepository
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ITraineeRepository _traineeRepository;
        private readonly IAssessmentScoreRepository _assessmentScoreRepository;
        private readonly ApplicationDbContext _dbContext;

        public IlpIntegrationRepository(IAssessmentRepository assessmentRepository,
                                            ITraineeRepository traineeRepository,
                                            IAssessmentScoreRepository assessmentScoreRepository,
                                             ApplicationDbContext dbContext)
        {
            _assessmentRepository = assessmentRepository;
            _traineeRepository = traineeRepository;
            _assessmentScoreRepository = assessmentScoreRepository;
            _dbContext = dbContext;
        }

        public async Task<(double AverageScore, int TotalScore)> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId)
        {
            var trainee = await _dbContext.Trainees.Include(t => t.User)
                                                  .FirstOrDefaultAsync(t => t.User.Email == traineeEmail);
            if (trainee == null)
            {
                return (0, 0);
            }

            var assessmentScore = await _dbContext.AssessmentScores
                                                .FirstOrDefaultAsync(a => a.TraineeId == trainee.TraineeId &&
                                                                          a.ScheduledAssessmentId == scheduledAssessmentId);
            if (assessmentScore == null)
            {
                return (0, 0);
            }

            var assessment = await _dbContext.Assessments
                                           .FirstOrDefaultAsync(a => a.AssessmentId == scheduledAssessmentId);
            if (assessment == null)
            {
                return (0, 0);
            }

            return (assessmentScore.AvergeScore, assessment.TotalScore ?? 0);
        }
        public async Task<IlpIntegrationScheduledAssessmentDTO> GetScheduledAssessmentDetails(string batchname)
        {
            var result = await (from sa in _dbContext.ScheduledAssessments
                                join a in _dbContext.Assessments on sa.AssessmentId equals a.AssessmentId
                                join b in _dbContext.batch on sa.BatchId equals b.batchid
                                join t in _dbContext.Trainers on a.CreatedBy equals t.TrainerId
                                join u in _dbContext.Users on t.UserId equals u.UserId
                                where b.batchname == batchname
                                select new IlpIntegrationScheduledAssessmentDTO
                                {
                                    BatchName = b.batchname,
                                    AssessmentId = sa.AssessmentId,
                                    AssessmentName = a.AssessmentName,
                                    CreatedByName = u.Username,
                                    startDateTime = sa.StartTime,
                                    endDateTime = sa.EndTime,
                                    Link = sa.Link,
                                    Status = sa.Status
                                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
