using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ReproProject.Models;
using ReproProject.ViewModels;
using System.Linq;

namespace ReproProject.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ReproContext _reproContext;
        private readonly IMapper _mapper;

        public EventsController(ReproContext reproContext, IMapper mapper)
        {
            _reproContext = reproContext;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<EventViewModel> Get([FromQuery(Name = "$filter")] string filter)
        {
            return _reproContext.Events
                .UseAsDataSource(_mapper.ConfigurationProvider)
                .For<EventViewModel>();
        }
    }
}
