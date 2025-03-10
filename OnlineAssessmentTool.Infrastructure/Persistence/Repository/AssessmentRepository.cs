﻿using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {


        public AssessmentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AssessmentOverviewDTO>> GetAllAssessmentOverviewsAsync()
        {
            return await (from sa in _context.ScheduledAssessments
                          join a in _context.Assessments on sa.AssessmentId equals a.AssessmentId
                          join t in _context.Trainers on a.CreatedBy equals t.TrainerId
                          join u in _context.Users on t.UserId equals u.UserId
                          join b in _context.batch on sa.BatchId equals b.batchid // Correct table name
                          orderby sa.ScheduledDate descending
                          group new { sa, a, t, u, b } by new
                          {
                              sa.ScheduledAssessmentId,
                              a.AssessmentName,
                              sa.ScheduledDate,
                              Trainer = u.Username,
                              BatchName = b.batchname, // Group by BatchName
                              sa.Status,
                          } into g
                          select new AssessmentOverviewDTO
                          {
                              AssessmentId = g.Key.ScheduledAssessmentId,
                              AssessmentName = g.Key.AssessmentName,
                              Date = g.Key.ScheduledDate,
                              Trainer = g.Key.Trainer,
                              BatchName = g.Key.BatchName, // Correctly map BatchName
                              Status = g.Key.Status.ToString(),
                          }).ToListAsync();
        }

        public async Task<IEnumerable<TraineeScoreDTO>> GetLowPerformersByAssessmentIdAsync(int scheduledAssessmentId)
        {
            var lowPerformers = await (
                from ats in _context.AssessmentScores
                where ats.ScheduledAssessmentId == scheduledAssessmentId
                join t in _context.Trainees on ats.TraineeId equals t.TraineeId
                join u in _context.Users on t.UserId equals u.UserId
                orderby ats.AvergeScore ascending
                select new TraineeScoreDTO
                {
                    TraineeName = u.Username,
                    Score = ats.AvergeScore
                }
            )
            .Take(5)
            .ToListAsync();

            return lowPerformers;
        }

        public async Task<IEnumerable<TraineeScoreDTO>> GetHighPerformersByAssessmentIdAsync(int scheduledAssessmentId)
        {
            var highPerformers = await (
                from u in _context.Users
                join t in _context.Trainees on u.UserId equals t.UserId
                join s in _context.AssessmentScores on t.TraineeId equals s.TraineeId
                join sa in _context.ScheduledAssessments on s.ScheduledAssessmentId equals sa.ScheduledAssessmentId
                where s.ScheduledAssessmentId == scheduledAssessmentId
                group new { u, s } by new { u.UserId, u.Username } into g
                select new TraineeScoreDTO
                {
                    TraineeName = g.Key.Username,
                    Score = g.Sum(x => x.s.AvergeScore)
                }
                into result
                orderby result.Score descending
                select result
            ).Take(5).ToListAsync();

            return highPerformers;
        }

        public async Task<List<AssessmentTableDTO>> GetAssessmentsForTrainer(int trainerId)
        {
            var result = from a in _context.Assessments
                         join sa in _context.ScheduledAssessments on a.AssessmentId equals sa.AssessmentId
                         join b in _context.batch on sa.BatchId equals b.batchid
                         join tb in _context.TrainerBatches on b.batchid equals tb.Batch_id
                         where tb.Trainer_id == trainerId
                         orderby sa.ScheduledDate descending
                         select new AssessmentTableDTO
                         {
                             ScheduledAssessmentId = sa.ScheduledAssessmentId,
                             AssessmentName = a.AssessmentName,
                             BatchName = b.batchname,
                             CreatedOn = a.CreatedOn,
                             ScheduledDate = sa.ScheduledDate,
                             Status = sa.Status.ToString()
                         };

            return await result.ToListAsync();
        }

        public async Task<List<TraineeAssessmentTableDTO>> GetTraineeAssessmentDetails(int scheduledAssessmentId)
        {
            var scheduledAssessment = await _context.ScheduledAssessments
                                                    .Where(sa => sa.ScheduledAssessmentId == scheduledAssessmentId)
                                                    .Select(sa => sa.BatchId)
                                                    .FirstOrDefaultAsync();

            if (scheduledAssessment == 0)
            {
                return new List<TraineeAssessmentTableDTO>();
            }

            var batchTrainees = await _context.Trainees
                                               .Where(t => t.BatchId == scheduledAssessment)
                                               .Include(t => t.User)
                                               .OrderBy(t => t.User.Username)
                                               .ToListAsync();

            var assessmentScores = await _context.AssessmentScores
                                                 .Where(a => a.ScheduledAssessmentId == scheduledAssessmentId)
                                                 .ToListAsync();

            var traineeScores = assessmentScores.ToDictionary(a => a.TraineeId, a => a.AvergeScore);

            var traineeDetails = new List<TraineeAssessmentTableDTO>();

            foreach (var trainee in batchTrainees)
            {
                var isCompleted = traineeScores.ContainsKey(trainee.TraineeId);
                var status = isCompleted ? AttendanceStatus.Completed : AttendanceStatus.Absent;
                var score = isCompleted ? traineeScores[trainee.TraineeId] : 0;

                traineeDetails.Add(new TraineeAssessmentTableDTO
                {
                    TraineeName = trainee.User.Username,
                    IsPresent = status.ToString(),
                    Score = score
                });
            }

            return traineeDetails;
        }

        private async Task<bool> CheckAttendanceStatus(int batchId, int traineeId, int scheduledAssessmentId)
        {
            var batchTrainees = await _context.Trainees
                                               .Where(t => t.BatchId == batchId)
                                               .Select(t => t.TraineeId)
                                               .ToListAsync();

            var scoredTrainees = await _context.AssessmentScores
                                                .Where(a => a.ScheduledAssessmentId == scheduledAssessmentId)
                                                .Select(a => a.TraineeId)
                                                .ToListAsync();

            return scoredTrainees.Contains(traineeId) && batchTrainees.Contains(traineeId);
        }
    }
}
