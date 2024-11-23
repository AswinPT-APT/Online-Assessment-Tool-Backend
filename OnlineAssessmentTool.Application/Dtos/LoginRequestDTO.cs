using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Dtos
{
    public class LoginRequestDTO
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
