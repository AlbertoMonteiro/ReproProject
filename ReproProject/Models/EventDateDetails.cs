using System;

namespace ReproProject.Models
{
    public class EventDateDetails
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public Event Event { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }
    }
}
