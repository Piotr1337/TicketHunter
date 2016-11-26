using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketHunter.Concrete;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;
using TicketHunter.Infrastructure;
using TicketHunter.Models;

namespace TicketHunter.Controllers
{
    [AllowAnonymous]
    public class EventController : Controller
    {
        private readonly IEventRepository _repository;
        private readonly ICategoryRepository _categoryRep;
        private readonly IArtistRepository _artistRep;
        private readonly ITicketRepository _ticketRep;


        public EventController(IEventRepository eventRepository, ICategoryRepository categoryRepository, IArtistRepository artistRepository, ITicketRepository ticketRepository)
        {
            this._repository = eventRepository;
            this._categoryRep = categoryRepository;
            this._artistRep = artistRepository;
            this._ticketRep = ticketRepository;
        }

        [ChildActionOnly]
        public ActionResult NavBar()
        {
            LuceneSearch.AddUpdateLuceneIndex(_repository.Events);

            NavbarViewModel model = new NavbarViewModel
            {
                Categories = _categoryRep.Categories,
                SubCategories = _categoryRep.SubCategories
            };

            return PartialView("NavBarSummary", model);
        }

        public async Task<ViewResult> List(int? categoryId,int? subcategoryId)
        {

            if (subcategoryId.HasValue)
            {
                EventsViewModel model = new EventsViewModel
                {
                    //Events = _repository.Events.Where(x => x.EventSubCategoryID == subcategoryId)
                    Events = await _repository.EventsAsync(null,subcategoryId)

                };
                return View(model);
            }
            else if(categoryId == null)
            {
                EventsViewModel model = new EventsViewModel
                {
                    Events =  await _repository.EventsAsync(null,null)
                };
                return View(model);
            }
            else
            {
                EventsViewModel model = new EventsViewModel
                {
                    Events = await _repository.EventsAsync(categoryId, null)
                };
                return View(model);
            }
        }

        public ViewResult ShowEvent(int? eventId)
        {
            List<Artists> getArtistsFromEvent = _ticketRep.Tickets
                .Where(x => x.EventID == eventId)
                .Select(ticket => _artistRep.GetArtists(ticket.ArtistID))
                .ToList();
            return View(new EventsViewModel()
            {
                Event = _repository.GetEvent(eventId),
                Artists = getArtistsFromEvent.Distinct().ToList(),
                Tickets = _ticketRep.Tickets
            });
        }

        public async Task<FileContentResult> GetImage(int? eventId)
        {
            var theEvent = await _repository.GetEventAsync(eventId);
            if (theEvent != null)
            {
                return File(theEvent.ImageData, theEvent.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public FileContentResult GetArtistImage(int artistId)
        {
            var theArtist = _artistRep.GetArtists(artistId);
            if (theArtist != null)
            {
                return File(theArtist.ImageData, theArtist.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> AutoCompleteSearch()
        {
            var events = await _repository.EventsAsync(null,null);
            //List<Artists> artists = _artistRep.Artists.ToList();


            var eventsResult = events.Select(s => new { Id = s.EventID, type = "wydarzenie", Name = s.EventName, Icon = s.Categories.Icon });
            //var artistsResult = artists.Select(a => new {Id = a.ArtistID, Name = a.Nickname});

            //var tt = new
            //{
            //    events = events.Select(s => new { Id = s.EventID, Name = s.EventName, Icon = s.Categories.Icon }),
            //    artists = artists.Select(a => new { Id = a.ArtistID, Name = a.Nickname })
            //};

            return Json(eventsResult, JsonRequestBehavior.AllowGet);
        }

    }
}