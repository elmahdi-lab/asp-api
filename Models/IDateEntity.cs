using System;

namespace PomeloHealthApi.Models
{
    public interface IDateEntity {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}