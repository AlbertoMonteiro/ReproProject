using AutoMapper;
using ReproProject.Models;
using ReproProject.ViewModels;
using System.Linq;

namespace ReproProject.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Dates.Min(x => x.StartDate)))
                .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(src => src.Dates.Max(x => x.FinishDate)))
                .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => $"{src.Organizer.LastName}, {src.Organizer.FirstName}"));
        }
    }
}
