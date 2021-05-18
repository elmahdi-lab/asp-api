using System;
using System.ComponentModel.DataAnnotations;

namespace PomeloHealthApi.Models
{
    public class Appointment : IDateEntity
    {
        [Key]
        public int Id {get; set;}

        public Patient Patient { get; set; }
        public Provider Provider { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}