using OnlineAssessmentTool.Domain.Entities;

namespace OnlineAssessmentTool.DataSeeder
{
    public static class TrainerBatchesSeeder
    {
        public static List<TrainerBatch> GetData()
        {
            return new List<TrainerBatch>()
            {
                new()
                {
                    Trainer_id = 1,
                    Batch_id = 1,
                },
                new()
                {
                    Trainer_id = 1,
                    Batch_id = 2,
                },
                new()
                {
                    Trainer_id = 1,
                    Batch_id = 3,
                },
                new()
                {
                    Trainer_id = 2,
                    Batch_id = 1,
                },
                new()
                {
                    Trainer_id = 2,
                    Batch_id = 2,
                },
                new()
                {
                    Trainer_id = 2,
                    Batch_id = 3,
                },
                new()
                {
                    Trainer_id = 2,
                    Batch_id = 4,
                },
            };
        }
    }
}
