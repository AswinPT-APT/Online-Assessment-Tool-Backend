﻿namespace OnlineAssessmentTool.Application.Dtos.Assessment
{
    public class TraineeAnswerDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int TraineeId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
    }
}
