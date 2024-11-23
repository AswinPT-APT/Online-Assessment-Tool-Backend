using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Permission> Permissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Users> Users { get; set; }
        DbSet<Batch> batch { get; set; }
        DbSet<Assessment> Assessments { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<QuestionOption> QuestionOptions { get; set; }
        DbSet<Trainer> Trainers { get; set; }
        DbSet<TrainerBatch> TrainerBatches { get; set; }
        DbSet<Trainee> Trainees { get; set; }
        DbSet<ScheduledAssessment> ScheduledAssessments { get; set; }
        DbSet<TraineeAnswer> TraineeAnswers { get; set; }
        DbSet<AssessmentScore> AssessmentScores { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DatabaseFacade Database { get; }
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        void RemoveRange(IEnumerable<object> entities);
        void AddRange(IEnumerable<object> entities);
    }
}
