﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAssessmentTool.Domain.Entities
{
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int batchid { get; set; }
        [Required]
        [StringLength(100)]
        public string batchname { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? isActive { get; set; }
        public ICollection<Trainee> Trainees { get; set; }
        public List<TrainerBatch> TrainerBatch { get; set; }
    }
}