using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Domain.Entities
{
    public class TraineeAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TraineeAnswerId { get; set; }
        [Required]
        [ForeignKey("ScheduledAssessment")]
        public int ScheduledAssessmentId { get; set; }
        [Required]
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
        public Question Question { get; set; }
    }
}
