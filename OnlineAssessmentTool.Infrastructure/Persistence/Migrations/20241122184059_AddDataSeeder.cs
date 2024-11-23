using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "PermissionName" },
                values: new object[,]
                {
                    { 1, "Upload questions to database", "UPLOAD_QUESTION" },
                    { 2, "Create an assessment and schedule to batches", "CREATE_ASSESSMENT" },
                    { 3, "Manage trainer accounts", "TRAINER_MANAGEMENT" },
                    { 4, "Manage user role and permissions", "ROLE_MANAGEMENT" },
                    { 5, "View performance details of all batches", "VIEW_PERFORMANCE_DETAILS" },
                    { 6, "Download and share performance reports", "DOWNLOAD_SHARE_REPORTS" },
                    { 7, "Evaluate an assessment and publish scores", "EVALUATE_TEST" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Trainer Manager" },
                    { 2, "External Trainer" },
                    { 3, "Internal Trainer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsAdmin", "Phone", "UUID", "UserType", "Username" },
                values: new object[,]
                {
                    { 1, "ashwin_admin@sreegcloudgmail.onmicrosoft.com", true, "9876543210", new Guid("0248591b-6c92-4205-95cd-8bca5474fd39"), 0, "Ashwin Admin" },
                    { 2, "rahul_admin@sreegcloudgmail.onmicrosoft.com", true, "9876543210", new Guid("e4e62d0f-82fa-49d5-b9cf-0d434f95ccc9"), 0, "Rahul Admin" },
                    { 3, "suneesh.thampi@sreegcloudgmail.onmicrosoft.com", false, "9876543210", new Guid("f5f0b3d1-1ffa-40b7-a7a7-afb0eec5eddd"), 1, "Suneesh Thampi" },
                    { 4, "lekshmi.a@sreegcloudgmail.onmicrosoft.com", false, "9876543210", new Guid("c237f64d-53f9-4b4e-9c54-4706135e1a90"), 1, "Lekshmi A" },
                    { 5, "revathy.rajeevan@sreegcloudgmail.onmicrosoft.com", false, "9876543210", new Guid("5bc7dd0a-1d7c-4865-a40c-fbe658575609"), 2, "Revathy Rajeevan" },
                    { 6, "rahul.s@sreegcloudgmail.onmicrosoft.com", false, "9876543210", new Guid("bb70bfec-179f-4849-81f9-af7a41dd4e83"), 2, "Rahul S" },
                    { 7, "emna.jose@sreegcloudgmail.onmicrosoft.com", false, "9876543210", new Guid("477701ad-2b6d-40f0-ac4c-e1196d067a73"), 2, "Emna Elizabeth Jose" }
                });

            migrationBuilder.InsertData(
                table: "batch",
                columns: new[] { "batchid", "CreatedOn", "batchname", "isActive" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(615), "ILP 2023-24 Batch 1", true },
                    { 2, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(625), "ILP 2023-24 Batch 2", true },
                    { 3, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(628), "ILP 2023-24 Batch 3", true },
                    { 4, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(629), "ILP 2023-24 Batch 4", true }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 5, 2 },
                    { 5, 3 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 1 },
                    { 7, 2 },
                    { 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "Trainees",
                columns: new[] { "TraineeId", "BatchId", "IsActive", "JoinedOn", "LastPasswordReset", "Password", "UserId" },
                values: new object[,]
                {
                    { 1, 3, true, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(1172), null, null, 5 },
                    { 2, 3, true, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(1180), null, null, 6 },
                    { 3, 3, true, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(1182), null, null, 7 }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "IsActive", "JoinedOn", "LastPasswordReset", "Password", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(1058), null, null, 1, 3 },
                    { 2, true, new DateTime(2024, 11, 22, 18, 40, 58, 937, DateTimeKind.Utc).AddTicks(1067), null, null, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "TrainerBatches",
                columns: new[] { "Batch_id", "Trainer_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "TraineeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "TraineeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "TraineeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "TrainerBatches",
                keyColumns: new[] { "Batch_id", "Trainer_id" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "batch",
                keyColumn: "batchid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "batch",
                keyColumn: "batchid",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "batch",
                keyColumn: "batchid",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "batch",
                keyColumn: "batchid",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);
        }
    }
}
