﻿namespace OnlineAssessmentTool.Application.Dtos.Assessment
{
    public class ScheduledAssessmentDetailsDTO
    {
        public int ScheduledAssessmentId { get; set; }
        public int MaximumScore { get; set; }
        public int TotalTrainees { get; set; }
        public int TraineesAttended { get; set; }
        public int Absentees { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AssessmentName { get; set; }
        public string BatchName { get; set; }
    }

}
