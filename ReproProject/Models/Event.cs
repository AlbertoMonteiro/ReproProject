using System.Collections.Generic;

namespace ReproProject.Models
{
    public class Event
    {
        public long Id { get; set; }
        public User Organizer { get; set; }
        public long? OrganizerId { get; set; }
        public string Contact { get; set; }
        public List<EventDateDetails> Dates { get; set; } = new List<EventDateDetails>();
    }
}
