using System;

namespace ReproProject.ViewModels
{
    public sealed class EventViewModel
    {
        public long Id { get; set; }
        public string Organizer { get; set; }
        public long? OrganizerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }
    }
}
