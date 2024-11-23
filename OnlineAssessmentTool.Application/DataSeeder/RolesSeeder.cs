using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class RolesSeeder
    {
        public static List<Role> GetData()
        {
            return new List<Role>
            {
                new()
                {
                    Id = 1,
                    RoleName = "Trainer Manager",
                },
                new()
                {
                    Id = 2,
                    RoleName = "External Trainer",
                },
                new()
                {
                    Id = 3,
                    RoleName = "Internal Trainer",
                },
            };
        }
    }
}
