﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Domain.Entities
{
    public class Trainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Users User { get; set; }
        [Required]
        public DateTime JoinedOn { get; set; }
        [StringLength(255)]
        public string? Password { get; set; }
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastPasswordReset { get; set; }
        public List<TrainerBatch> TrainerBatch { get; set; }
    }
}
