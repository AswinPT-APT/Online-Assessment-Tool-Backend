using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Application.Dtos.Assessment
{
    public class QuestionOptionDTO
    {
        [Column(TypeName = "jsonb")]
        public List<string> Options { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> CorrectAnswers { get; set; }
    }
}
