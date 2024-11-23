using OnlineAssessmentTool.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Domain.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        public Guid UUID { get; set; }
        [Required]
        public UserType UserType { get; set; }
    }
}
