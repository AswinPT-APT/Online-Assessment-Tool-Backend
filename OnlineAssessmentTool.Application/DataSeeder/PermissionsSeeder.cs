using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class PermissionsSeeder
    {
        public static List<Permission> GetData()
        {
            return new List<Permission>
            {
                new()
                {
                    Id = 1,
                    PermissionName = "UPLOAD_QUESTION",
                    Description = "Upload questions to database",
                },
                new()
                {
                    Id = 2,
                    PermissionName = "CREATE_ASSESSMENT",
                    Description = "Create an assessment and schedule to batches",
                },
                new()
                {
                    Id = 3,
                    PermissionName = "TRAINER_MANAGEMENT",
                    Description = "Manage trainer accounts",
                },
                new()
                {
                    Id = 4,
                    PermissionName = "ROLE_MANAGEMENT",
                    Description = "Manage user role and permissions",
                },
                new()
                {
                    Id = 5,
                    PermissionName = "VIEW_PERFORMANCE_DETAILS",
                    Description = "View performance details of all batches",
                },
                new()
                {
                    Id = 6,
                    PermissionName = "DOWNLOAD_SHARE_REPORTS",
                    Description = "Download and share performance reports",
                },
                new()
                {
                    Id = 7,
                    PermissionName = "EVALUATE_TEST",
                    Description = "Evaluate an assessment and publish scores",
                },
            };
        }
    }
}
