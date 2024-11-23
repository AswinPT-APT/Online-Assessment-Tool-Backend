using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class TraineesSeeder
    {
        public static List<Trainee> GetData()
        {
            return new List<Trainee>
            {
                new()
                {
                    TraineeId = 1,
                    UserId = 5,
                    JoinedOn = DateTime.UtcNow,
                    BatchId = 3,
                    IsActive = true,
                },
                new()
                {
                    TraineeId = 2,
                    UserId = 6,
                    JoinedOn = DateTime.UtcNow,
                    BatchId = 3,
                    IsActive = true,
                },
                new()
                {
                    TraineeId = 3,
                    UserId = 7,
                    JoinedOn = DateTime.UtcNow,
                    BatchId = 3,
                    IsActive = true,
                },
            };
        }
    }
}
