using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class BatchSeeder
    {
        public static List<Batch> GetData()
        {
            return new List<Batch>
            {
                new()
                {
                    batchid = 1,
                    batchname = "ILP 2023-24 Batch 1",
                    CreatedOn = DateTime.UtcNow,
                    isActive = true,
                },
                new()
                {
                    batchid = 2,
                    batchname = "ILP 2023-24 Batch 2",
                    CreatedOn = DateTime.UtcNow,
                    isActive = true,
                },
                new()
                {
                    batchid = 3,
                    batchname = "ILP 2023-24 Batch 3",
                    CreatedOn = DateTime.UtcNow,
                    isActive= true,
                },
                new()
                {
                    batchid = 4,
                    batchname = "ILP 2023-24 Batch 4",
                    CreatedOn = DateTime.UtcNow,
                    isActive = true,
                },
            };
        }
    }
}
