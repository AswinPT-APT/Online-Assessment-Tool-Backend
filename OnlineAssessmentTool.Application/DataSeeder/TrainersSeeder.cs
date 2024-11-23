using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class TrainersSeeder
    {
        public static List<Trainer> GetData()
        {
            return new List<Trainer>
            {
                new()
                {
                    TrainerId = 1,
                    UserId = 3,
                    JoinedOn = DateTime.UtcNow,
                    RoleId = 1,
                    IsActive = true,
                },
                new()
                {
                    TrainerId = 2,
                    UserId = 4,
                    JoinedOn = DateTime.UtcNow,
                    RoleId = 2,
                    IsActive = true,
                },
            };
        }
    }
}
