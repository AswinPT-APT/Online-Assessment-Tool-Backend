﻿using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Dtos.Assessment;
using OnlineAssessmentTool.Application.Dtos.User;
using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Repository
{
    public class ScheduledAssessmentRepository : Repository<ScheduledAssessment>, IScheduledAssessmentRepository
    {


        public ScheduledAssessmentRepository(ApplicationDbContext context) : base(context)
        {


        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAttendedStudentsAsync(int scheduledAssessmentId)
        {
            return await _context.TraineeAnswers
       .Where(ta => ta.ScheduledAssessmentId == scheduledAssessmentId)
       .Select(ta => new TraineeStatusDTO
       {
           TraineeId = ta.TraineeId,
           // Fetch Username from Users table
           Name = _context.Trainees
                         .Where(t => t.TraineeId == ta.TraineeId)
                         .Select(t => t.User.Username)
                         .FirstOrDefault(),
           Score = _context.AssessmentScores
                         .Where(a => a.TraineeId == ta.TraineeId && a.ScheduledAssessmentId == ta.ScheduledAssessmentId)
                         .Select(a => a.AvergeScore)
                         .FirstOrDefault()
       })
       .Distinct()
       .ToListAsync();
        }

        public async Task<IEnumerable<TraineeStatusDTO>> GetAbsentStudentsAsync(int scheduledAssessmentId)
        {
            var batchId = await _context.ScheduledAssessments
                .Where(sa => sa.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(sa => sa.BatchId)
                .FirstOrDefaultAsync();

            var attendedTraineeIds = await _context.TraineeAnswers
                .Where(ta => ta.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(ta => ta.TraineeId)
                .ToListAsync();

            var absentTrainees = await _context.Trainees
                .Where(t => t.BatchId == batchId && !attendedTraineeIds.Contains(t.TraineeId))
                .Select(t => new TraineeStatusDTO
                {
                    TraineeId = t.TraineeId,
                    Name = t.User.Username
                })
                .ToListAsync();

            return absentTrainees;
        }

        public async Task<IEnumerable<TraineeAnswerDetailDTO>> GetTraineeAnswerDetailsAsync(int traineeId, int scheduledAssessmentId)
        {
            var result = await _context.TraineeAnswers
                .Where(ta => ta.TraineeId == traineeId && ta.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(ta => new
                {
                    ta.QuestionId,
                    ta.Answer,
                    ta.IsCorrect,
                    ta.Score,
                    QuestionText = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.QuestionText)
                        .FirstOrDefault(),
                    QuestionType = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.QuestionType)
                        .FirstOrDefault(),
                    Points = _context.Questions
                        .Where(q => q.QuestionId == ta.QuestionId)
                        .Select(q => q.Points)
                        .FirstOrDefault(),
                    QuestionOptions = _context.QuestionOptions
                        .Where(o => o.QuestionId == ta.QuestionId)
                        .Select(o => new QuestionOptionDTO
                        {
                            Options = o.Options,
                            CorrectAnswers = o.CorrectAnswers
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return result.Select(item => new TraineeAnswerDetailDTO
            {
                QuestionId = item.QuestionId,
                Answer = item.Answer,
                IsCorrect = item.IsCorrect,
                Score = item.Score,
                QuestionText = item.QuestionText,
                QuestionType = item.QuestionType,
                Points = item.Points,
                QuestionOptions = item.QuestionOptions
            });
        }
        public async Task<int> GetStudentCountByAssessmentIdAsync(int assessmentId)
        {
            var batchIds = await _context.ScheduledAssessments
                .Where(sa => sa.AssessmentId == assessmentId)
                .Select(sa => sa.BatchId)
                .Distinct()
                .ToListAsync();

            var totalStudents = await _context.batch
                .Where(b => batchIds.Contains(b.batchid))
                .SelectMany(b => b.Trainees)
                .CountAsync();

            return totalStudents;
        }

        public async Task<List<GetScheduledAssessmentDTO>> GetScheduledAssessmentsByUserIdAsync(int userId)
        {
            var traineeBatchIds = await _context.Trainees
                .Where(t => t.TraineeId == userId)
                .Select(t => t.BatchId)
                .ToListAsync();

            var scheduledAssessments = await _context.ScheduledAssessments
                .Where(sa => traineeBatchIds.Contains(sa.BatchId))
                .Select(sa => new GetScheduledAssessmentDTO
                {
                    BatchId = sa.BatchId,
                    AssessmentId = sa.AssessmentId,
                    ScheduledAssessmentId = sa.ScheduledAssessmentId,
                    AssessmentName = _context.Assessments
                        .Where(a => a.AssessmentId == sa.AssessmentId)
                        .Select(a => a.AssessmentName)
                        .FirstOrDefault(),
                    ScheduledDate = sa.ScheduledDate,
                    AssessmentDuration = sa.AssessmentDuration,
                    StartDate = sa.StartDate,
                    EndDate = sa.EndDate,
                    StartTime = sa.StartTime,
                    EndTime = sa.EndTime,
                    Status = sa.Status,
                    CanRandomizeQuestion = sa.CanRandomizeQuestion,
                    CanDisplayResult = sa.CanDisplayResult,
                    CanSubmitBeforeEnd = sa.CanSubmitBeforeEnd
                })
                .ToListAsync();

            return scheduledAssessments;
        }

        public async Task<AssessmentTableDTO> GetAssessmentTableByScheduledAssessmentId(int scheduledAssessmentId)
        {
            var result = await (from a in _context.Assessments
                                join sa in _context.ScheduledAssessments on a.AssessmentId equals sa.AssessmentId
                                join b in _context.batch on sa.BatchId equals b.batchid
                                where sa.ScheduledAssessmentId == scheduledAssessmentId
                                select new AssessmentTableDTO
                                {
                                    AssessmentId = sa.AssessmentId,
                                    AssessmentName = a.AssessmentName,
                                    BatchName = b.batchname,
                                    CreatedOn = a.CreatedOn,
                                    ScheduledDate = sa.ScheduledDate,
                                    Status = sa.Status.ToString()
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ScheduledAssessmentDetailsDTO> GetScheduledAssessmentDetailsAsync(int scheduledAssessmentId)
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

            var batchId = scheduledAssessment.BatchId;

            var totalTrainees = await _context.batch
                .Where(b => b.batchid == batchId)
                .SelectMany(b => b.Trainees)
                .CountAsync();

            var traineesAttended = await _context.AssessmentScores
                .Where(aa => aa.ScheduledAssessmentId == scheduledAssessmentId)
                .Select(aa => aa.TraineeId)
                .Distinct()
                .CountAsync();

            var absentees = totalTrainees - traineesAttended;

            var assessmentName = assessment.AssessmentName;

            var batchDetails = await _context.batch
                .FirstOrDefaultAsync(b => b.batchid == batchId);

            var batchName = batchDetails.batchname;

            return new ScheduledAssessmentDetailsDTO
            {
                ScheduledAssessmentId = scheduledAssessmentId,
                MaximumScore = maximumScore,
                TotalTrainees = totalTrainees,
                TraineesAttended = traineesAttended,
                Absentees = absentees,
                AssessmentDate = scheduledAssessment.ScheduledDate,
                AssessmentName = assessmentName,
                BatchName = batchName
            };
        }
    }
}

