using System;
using System.ComponentModel.DataAnnotations;;

namespace PomeloHealthApi.Models
{
    public class Availability : IDateEntity
    {
        public int Id {get; set;}
        public Provider Provider { get; set; } = null;

        [Required]
        public DateTime StartDate { get; set; }        
        [Required]
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}