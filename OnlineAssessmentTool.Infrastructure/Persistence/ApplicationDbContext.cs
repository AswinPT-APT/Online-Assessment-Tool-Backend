using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Application.Common.Interfaces;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int affectedRecords = await base.SaveChangesAsync(cancellationToken);
                return affectedRecords > 0;
            }
            catch
            {
                return false;
            }
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Batch> batch { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerBatch> TrainerBatches { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<ScheduledAssessment> ScheduledAssessments { get; set; }
        public DbSet<TraineeAnswer> TraineeAnswers { get; set; }
        public DbSet<AssessmentScore> AssessmentScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
              .Property(r => r.Id)
              .UseIdentityColumn();

            modelBuilder.Entity<TrainerBatch>()
                 .HasKey(ba => new { ba.Trainer_id, ba.Batch_id });

            modelBuilder.Entity<TrainerBatch>()
                      .HasOne(ba => ba.Trainer)
                      .WithMany(b => b.TrainerBatch)
                      .HasForeignKey(ba => ba.Trainer_id);

            modelBuilder.Entity<TrainerBatch>()
                    .HasOne(ba => ba.Batch)
                    .WithMany(b => b.TrainerBatch)
                    .HasForeignKey(ba => ba.Batch_id);

            modelBuilder.Entity<Users>().ToTable("Users");

            modelBuilder.Entity<Role>().ToTable("Roles");

            modelBuilder.Entity<Trainer>().ToTable("Trainers");

            modelBuilder.Entity<Trainer>()
               .HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Role)
                .WithMany()
                .HasForeignKey(t => t.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.SeedData();

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    j => j.HasOne<Permission>()
                          .WithMany()
                          .HasForeignKey("PermissionId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Role>()
                          .WithMany()
                          .HasForeignKey("RoleId")
                          .OnDelete(DeleteBehavior.Cascade))
                .HasData(
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 1 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 2 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 3 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 4 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 5 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 6 } },
                   new Dictionary<string, object> { { "RoleId", 1 }, { "PermissionId", 7 } },
                   new Dictionary<string, object> { { "RoleId", 2 }, { "PermissionId", 1 } },
                   new Dictionary<string, object> { { "RoleId", 2 }, { "PermissionId", 2 } },
                   new Dictionary<string, object> { { "RoleId", 2 }, { "PermissionId", 5 } },
                   new Dictionary<string, object> { { "RoleId", 2 }, { "PermissionId", 6 } },
                   new Dictionary<string, object> { { "RoleId", 2 }, { "PermissionId", 7 } },
                   new Dictionary<string, object> { { "RoleId", 3 }, { "PermissionId", 1 } },
                   new Dictionary<string, object> { { "RoleId", 3 }, { "PermissionId", 2 } },
                   new Dictionary<string, object> { { "RoleId", 3 }, { "PermissionId", 5 } },
                   new Dictionary<string, object> { { "RoleId", 3 }, { "PermissionId", 6 } },
                   new Dictionary<string, object> { { "RoleId", 3 }, { "PermissionId", 7 } }
               );
        }
    }

}
