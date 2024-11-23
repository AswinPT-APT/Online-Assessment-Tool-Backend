using OnlineAssessmentTool.Shared.Enums;

namespace OnlineAssessmentTool.Application.Dtos.Assessment
{
    public class ScheduledAssessmentDTO
    {
        public int BatchId { get; set; }
        public int AssessmentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string AssessmentDuration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AssessmentStatus Status { get; set; }
        public bool CanRandomizeQuestion { get; set; }
        public bool CanDisplayResult { get; set; }
        public bool CanSubmitBeforeEnd { get; set; }
        public string Link { get; set; }
    }
}

