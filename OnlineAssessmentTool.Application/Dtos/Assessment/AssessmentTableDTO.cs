﻿namespace OnlineAssessmentTool.Application.Dtos.Assessment
{
    public class AssessmentTableDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string BatchName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public enum AssessmentStatus
        {
            InProgress,
            Completed,
            Upcoming,
            Cancelled
        }
    }
}
