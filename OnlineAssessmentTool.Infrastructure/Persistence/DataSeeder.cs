using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.DataSeeder;
using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.Persistence
{
    public static class DataSeeder
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedBatch();
            modelBuilder.SeedPermissions();
            modelBuilder.SeedRoles();
            modelBuilder.SeedUsers();
            modelBuilder.SeedTrainers();
            modelBuilder.SeedTrainerBatches();
            modelBuilder.SeedTrainees();
        }

        private static void SeedBatch(this ModelBuilder modelBuilder)
        {
            var data = BatchSeeder.GetData();
            modelBuilder.Entity<Batch>().HasData(data);
        }

        private static void SeedPermissions(this ModelBuilder modelBuilder)
        {
            var data = PermissionsSeeder.GetData();
            modelBuilder.Entity<Permission>().HasData(data);
        }

        private static void SeedRoles(this ModelBuilder modelBuilder)
        {
            var data = RolesSeeder.GetData();
            modelBuilder.Entity<Role>().HasData(data);
        }

        private static void SeedUsers(this ModelBuilder modelBuilder)
        {
            var data = UsersSeeder.GetData();
            modelBuilder.Entity<Users>().HasData(data);
        }

        private static void SeedTrainers(this ModelBuilder modelBuilder)
        {
            var data = TrainersSeeder.GetData();
            modelBuilder.Entity<Trainer>().HasData(data);
        }

        private static void SeedTrainerBatches(this ModelBuilder modelBuilder)
        {
            var data = TrainerBatchesSeeder.GetData();
            modelBuilder.Entity<TrainerBatch>().HasData(data);
        }

        private static void SeedTrainees(this ModelBuilder modelBuilder)
        {
            var data = TraineesSeeder.GetData();
            modelBuilder.Entity<Trainee>().HasData(data);
        }
    }
}
