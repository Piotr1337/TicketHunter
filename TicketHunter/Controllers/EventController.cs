﻿using System.Collections.Generic;
using System.Diagnostics;
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

        public ViewResult List(int? categoryId,int? subcategoryId)
        {

            if (subcategoryId.HasValue)
            {
                EventsViewModel model = new EventsViewModel
                {
                    Events = _repository.EventsAsync(null,subcategoryId)
                };
                return View(model);
            }
            else if(categoryId == null)
            {
                EventsViewModel model = new EventsViewModel
                {
                    Events =  _repository.EventsAsync(null,null)
                };
                return View(model);
            }
            else
            {
                EventsViewModel model = new EventsViewModel
                {
                    Events = _repository.EventsAsync(categoryId, null)
                };
                return View(model);
            }
        }

        public ViewResult ShowEvent(int? eventId)
        {
            List<Artists> artists = new List<Artists>();
            var tickets = _ticketRep.Tickets.Where(x => x.EventID == eventId);
            foreach (var ti in tickets)
            {
                var ticket = _ticketRep.TicketArtists.Where(x => x.TicketID == ti.TicketID);
                foreach (var ar in ticket)
                {
                    var art = _artistRep.GetArtists(ar.ArtistID);
                    artists.Add(art);
                }
            }
            return View(new EventsViewModel()
            {
                Event = _repository.GetEvent(eventId),
                Artists = artists,
                Tickets = _ticketRep.Tickets
            });
        }

        public FileContentResult GetImage(int? eventId)
        {
            var theEvent = _repository.GetEventAsync(eventId);
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
        public JsonResult AutoCompleteSearch(string phrase)
        {
            //var res = LuceneSearch.Search(phrase, "");
            
            //foreach (var item in res)
            //{
            //    Debug.WriteLine("Rezultat:" + item.EventName);
            //}
            List<SearchViewModel> serachResult = new List<SearchViewModel>();
            var events = _repository.Events.ToList();
            var artists = _artistRep.Artists.ToList();

            foreach (var item in events)
            {
                SearchViewModel model = new SearchViewModel();
                model.Name = item.EventName;
                model.Type = "event";
                model.Icon = item.Categories.Icon;
                model.Id = item.EventID;
                serachResult.Add(model);
            }
            foreach (var item in artists)
            {
                SearchViewModel model = new SearchViewModel();
                model.Name = item.Nickname;
                model.Type = "artist";
                model.Icon = "glyphicon glyphicon-user";
                model.Id = item.ArtistID;
                serachResult.Add(model);
            }


            //var result = res.Select(s => new { Name = s.EventName, Type = "Event", Id = s.EventID });

            var result = serachResult.Select(s => new { Name = s.Name, Type = s.Type, Icon = s.Icon, Id = s.Id });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}