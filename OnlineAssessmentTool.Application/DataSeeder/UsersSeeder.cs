using OnlineAssessmentTool.Domain.Entities;
using OnlineAssessmentTool.Shared.Enums;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class UsersSeeder
    {
        public static List<Users> GetData()
        {
            return new List<Users>()
            {
                new()
                {
                    UserId = 1,
                    Username = "Ashwin Admin",
                    Email = "ashwin_admin@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = true,
                    UUID = Guid.NewGuid(),
                    UserType = 0,
                },
                new()
                {
                    UserId = 2,
                    Username = "Rahul Admin",
                    Email = "rahul_admin@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = true,
                    UUID = Guid.NewGuid(),
                    UserType = 0,
                },
                new()
                {
                    UserId = 3,
                    Username = "Suneesh Thampi",
                    Email = "suneesh.thampi@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = false,
                    UUID = Guid.NewGuid(),
                    UserType = UserType.Trainer,
                },
                new()
                {
                    UserId = 4,
                    Username = "Lekshmi A",
                    Email = "lekshmi.a@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = false,
                    UUID = Guid.NewGuid(),
                    UserType = UserType.Trainer,
                },
                new()
                {
                    UserId = 5,
                    Username = "Revathy Rajeevan",
                    Email = "revathy.rajeevan@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = false,
                    UUID = Guid.NewGuid(),
                    UserType = UserType.Trainee,
                },
                new()
                {
                    UserId = 6,
                    Username = "Rahul S",
                    Email = "rahul.s@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = false,
                    UUID = Guid.NewGuid(),
                    UserType = UserType.Trainee,
                },
                new()
                {
                    UserId = 7,
                    Username = "Emna Elizabeth Jose",
                    Email = "emna.jose@sreegcloudgmail.onmicrosoft.com",
                    Phone = "9876543210",
                    IsAdmin = false,
                    UUID = Guid.NewGuid(),
                    UserType = UserType.Trainee,
                },
            };
        }
    }
}
