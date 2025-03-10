﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentScoreRepository : Repository<AssessmentScore>, IAssessmentScoreRepository
    {
        public AssessmentScoreRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ICollection<AssessmentScoreGraphDTO>> GetAssessmentByIdAsync(int id)
        {
            var assessmentScores = await _context.AssessmentScores
                                     .Where(a => a.ScheduledAssessmentId == id)
                                     .Include(a => a.ScheduledAssessment) // Include ScheduledAssessment
                                     .ThenInclude(sa => sa.Assessment) // Include Assessment within ScheduledAssessment
                                     .Select(a => new AssessmentScoreGraphDTO
                                     {
                                         TraineeId = a.TraineeId,
                                         AvergeScore = a.AvergeScore,
                                         TotalScore = a.ScheduledAssessment.Assessment.TotalScore ?? 0 // Default to 0 if null
                                     })
                                     .ToListAsync();
            return assessmentScores;
        }

        public async Task<List<TraineeAssessmentScoreDTO>> GetAssessmentScoresByTraineeIdAsync(int traineeId)
        {
            var query = from ass in _context.AssessmentScores
                        join sa in _context.ScheduledAssessments
                        on ass.ScheduledAssessmentId equals sa.ScheduledAssessmentId
                        join a in _context.Assessments
                        on sa.AssessmentId equals a.AssessmentId
                        where ass.TraineeId == traineeId
                        select new TraineeAssessmentScoreDTO
                        {
                            AssessmentScoreId = ass.AssessmentScoreId,
                            ScheduledAssessmentId = ass.ScheduledAssessmentId,
                            AssessmentId = sa.AssessmentId,
                            AssessmentName = a.AssessmentName,
                            ScheduledDate = sa.ScheduledDate,
                            Score = ass.AvergeScore,
                            CalculatedOn = ass.CalculatedOn
                        };

            return await query.ToListAsync();
        }

        public async Task<AssessmentScore> GetByScheduledAssessmentAndTraineeAsync(int scheduledAssessmentId, int traineeId)
        {
            return await _context.AssessmentScores
                .FirstOrDefaultAsync(a => a.ScheduledAssessmentId == scheduledAssessmentId && a.TraineeId == traineeId);
        }

        public async Task UpdateAssessmentScoresAsync(List<AssessmentScoreDTO> assessmentScoreDTOs)
        {
            foreach (var assessmentScoreDTO in assessmentScoreDTOs)
            {
                var assessmentScore = await GetByScheduledAssessmentAndTraineeAsync(assessmentScoreDTO.ScheduledAssessmentId, assessmentScoreDTO.TraineeId);

                if (assessmentScore == null)
                {
                    // If the assessment score record doesn't exist, continue to the next one
                    continue;
                }

                // Update the assessment score
                assessmentScore.AvergeScore = assessmentScoreDTO.AvergeScore;
                assessmentScore.CalculatedOn = DateTime.UtcNow;

                _context.AssessmentScores.Update(assessmentScore);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetScoreDistributionAsync(int scheduledAssessmentId)
        {
            var scheduledAssessment = await _context.ScheduledAssessments
                .FirstOrDefaultAsync(sa => sa.ScheduledAssessmentId == scheduledAssessmentId);

            if (scheduledAssessment == null)
            {
                throw new Exception("Scheduled assessment not found");
            }

            var assessmentId = scheduledAssessment.AssessmentId;
            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.AssessmentId == assessmentId);

            if (assessment == null)
            {
                throw new Exception("Assessment not found");
            }

            var maximumScore = assessment.TotalScore ?? 0;

            if (maximumScore == 0)
            {
                throw new Exception("Maximum score cannot be zero");
            }

            var scoreDistribution = await _context.AssessmentScores
                .Where(a => a.ScheduledAssessmentId == scheduledAssessmentId)
                .GroupBy(a => new
                {
                    Category = ((double)a.AvergeScore / maximumScore) * 100 >= 90 ? "Above 90%" :
                               ((double)a.AvergeScore / maximumScore) * 100 >= 80 ? "80% - 90%" :
                               ((double)a.AvergeScore / maximumScore) * 100 >= 70 ? "70% - 80%" :
                               ((double)a.AvergeScore / maximumScore) * 100 >= 60 ? "60% - 70%" : "Below 60%"
                })
                .Select(g => new
                {
                    Category = g.Key.Category,
                    Count = g.Count(),
                    ScoreOrder = g.Key.Category == "Above 90%" ? 5 :
                                 g.Key.Category == "80% - 90%" ? 4 :
                                 g.Key.Category == "70% - 80%" ? 3 :
                                 g.Key.Category == "60% - 70%" ? 2 : 1
                })
                .OrderByDescending(g => g.ScoreOrder)
                .Select(g => new
                {
                    Category = g.Category,
                    Count = g.Count
                })
                .ToListAsync();

            return scoreDistribution;
        }

        public async Task<ActionResult<IEnumerable<TraineeAverageScoreDto>>> GetTraineesWithAverageScore(string batchName)
        {
            var batch = await _context.batch
                .Include(b => b.Trainees)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(b => b.batchname == batchName);

            if (batch == null)
            {
                return null;
            }

            var scheduledAssessments = await _context.ScheduledAssessments
                .Where(sa => sa.BatchId == batch.batchid)
                .Include(sa => sa.Assessment)
                .ToListAsync();

            var totalAssessmentsScheduled = scheduledAssessments.Count;

            var batchScores = await _context.ScheduledAssessments
                .Where(sa => sa.BatchId == batch.batchid)
                .SelectMany(sa => _context.AssessmentScores
                    .Where(ass => ass.ScheduledAssessmentId == sa.ScheduledAssessmentId)
                    .Select(ass => new
                    {
                        Score = (double)ass.AvergeScore,
                        TotalScore = (double)sa.Assessment.TotalScore
                    }))
                .ToListAsync();

            var totalScoreObtained = batchScores.Sum(scoreData => scoreData.Score);
            var totalMaxScore = batchScores.Sum(scoreData => scoreData.TotalScore);

            var batchAverageScore = totalScoreObtained / totalMaxScore;
            var batchAverageScorePercentage = batchAverageScore * 100;

            var traineeAverageScores = batch.Trainees
                .Select(t =>
                {
                    var traineeScores = _context.AssessmentScores
                        .Where(ass => ass.TraineeId == t.TraineeId && scheduledAssessments.Select(sa => sa.ScheduledAssessmentId).Contains(ass.ScheduledAssessmentId))
                        .ToList();

                    var totalScore = traineeScores.Sum(ass => ass.AvergeScore);
                    var totalAssessmentsCompleted = traineeScores.Count;
                    var averagePercentageScore = _context.AssessmentScores
                        .Where(ass => ass.TraineeId == t.TraineeId)
                        .Average(ass =>
                            (double)ass.AvergeScore / _context.Assessments
                            .Where(a => a.AssessmentId == ass.ScheduledAssessment.AssessmentId)
                            .Select(a => a.TotalScore)
                            .FirstOrDefault() * 100) ?? 0.0;

                    var lastAssessmentDate = traineeScores
                        .OrderByDescending(ass => ass.CalculatedOn)
                        .FirstOrDefault()?.CalculatedOn;

                    return new TraineeAverageScoreDto
                    {
                        TraineeId = t.TraineeId,
                        TraineeName = t.User.Username,
                        BatchName = batchName,
                        AveragePercentageScore = averagePercentageScore,
                        TotalAssessmentsCompleted = totalAssessmentsCompleted,
                        TotalScore = totalScore,
                        LastAssessmentDate = lastAssessmentDate ?? DateTime.MinValue,
                        RankInBatch = 0,
                        BatchAverageScore = batchAverageScorePercentage,
                    };
                })
                .ToList();

            traineeAverageScores = traineeAverageScores
                .OrderByDescending(tas => tas.AveragePercentageScore)
                .Select((tas, index) =>
                {
                    tas.RankInBatch = index + 1;
                    return tas;
                })
                .ToList();

            return (traineeAverageScores);
        }
    }
}
