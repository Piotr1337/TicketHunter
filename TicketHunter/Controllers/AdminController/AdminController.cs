using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;
using TicketHunter.Infrastructure;
using TicketHunter.Models;
using TicketHunter.Models.Admin;
using TicketReservation.Models;
using System.Timers;

namespace TicketHunter.Controllers.AdminController
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IEventRepository _repository;
        private readonly ICategoryRepository _catRepo;
        private readonly ITicketRepository _ticketRepository;
        private readonly IArtistRepository _artistRepository;


        public AdminController(IEventRepository repo, ICategoryRepository catRepository, ITicketRepository ticketRepo, IArtistRepository artistRepo)
        {
            _repository = repo;
            _catRepo = catRepository;
            _ticketRepository = ticketRepo;
            _artistRepository = artistRepo;
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [AllowAnonymous]
        public ViewResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminLogin(AuthModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.LoginModel.Email);
            if (user != null)
            {
                if (UserManager.IsInRole(user.Id, "Admin"))
                {
                    var result = await
                        SignInManager.PasswordSignInAsync(model.LoginModel.Email, model.LoginModel.Password,
                            model.LoginModel.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            //return RedirectToLocal(model.ReturnUrl);
                            return Redirect(Url.Action("Index", "Admin") + "#tile");
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode",
                                new { ReturnUrl = model.ReturnUrl, RememberMe = model.LoginModel.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
                            return View("AdminLogin", model);
                    }
                }
                ModelState.AddModelError("", "Nie masz uprawnień");
            }
            else
            {
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło");
            }
            return View("AdminLogin", model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("AdminLogin", "Admin");
        }

        // GET: Admin
        public ViewResult Index()
        {
            IndexAdminViewModel model = new IndexAdminViewModel()
            {
                IndexEvents = _repository.Events,
                IndexArtists = _artistRepository.Artists,
                
            };
            return View(model);
        }

        //EVENTY
        public ViewResult Edit(int eventId)
        {

            var theEvent = _repository.Events.FirstOrDefault(x => x.EventID == eventId);
            var viewModel = Mapper.Map<Events, AdminViewModel>(theEvent);

            viewModel.CategoriesForDropList = _catRepo.CategoriesForDropList;
            viewModel.SubCategoryForDropList = PopulateSubCategory(viewModel.EventCategoryID);
            viewModel.Events = _repository.Events.FirstOrDefault(x => x.EventID == eventId);
            viewModel.ShowCalendar = true;
            ViewBag.IsAdmin = true;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int eventId)
        {
            Events deletedEvent = _repository.DeleteEvent(eventId);
            if (deletedEvent != null)
            {
                TempData["ticketMessage"] = $"Usunięto {deletedEvent.EventName}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Events theEvent, HttpPostedFileBase image = null)
        {
            var viewModel = Mapper.Map<Events, AdminViewModel>(theEvent);
            if (ModelState.IsValid)
            {
                if (image != null)
                {                 
                    viewModel.ImageMimeType = image.ContentType;
                    viewModel.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(viewModel.ImageData, 0, image.ContentLength);
                }
                ViewData["Category"] = viewModel;
                var toModel = Mapper.Map<AdminViewModel, Events>(viewModel);
                toModel.Published = false;
                _repository.SaveEvent(toModel);
                TempData["message"] = $"Zapisano {theEvent.EventName}";
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new AdminViewModel()
            {
                EventStartDateTime = DateTime.Now,
                EventEndDateTime = DateTime.Now,
                PublicationDate = DateTime.Now,
                CategoriesForDropList = _catRepo.CategoriesForDropList,
                SubCategoryForDropList = new[] { new SelectListItem { Value = "", Text = "" } },
                Events = new Events(),
                ShowCalendar = false
            });
        }

        //BILETY
        public ViewResult AddTicket(string date, int eventId)
        {
            return View("TicketEdit", new TicketViewModel()
            {
                DateOfEvent = DateTime.Parse(date),
                ArtistList = _artistRepository.ArtistsForDropList,
                EventID = eventId
            });
        }

        public ActionResult DeleteTicket(int ticketId)
        {
            Ticket deletedTicket = _ticketRepository.DeleteTicket(ticketId);
            if (deletedTicket != null)
            {
                TempData["ticketMessage"] = $"Usunięto {deletedTicket.Title}";
                if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return View("Index");
        }

        public ViewResult TicketEdit(int ticketId)
        {
            var theTicket = _ticketRepository.Tickets.FirstOrDefault(x => x.TicketID == ticketId);
            var viewModel = Mapper.Map<Ticket, TicketViewModel>(theTicket);

            return View(viewModel);
        }

        public IEnumerable<Ticket> LoadAllTickets()
        {
            var result = _ticketRepository.Tickets;

            return result.Select(item => new Ticket
            {
                EventID = item.EventID,
                TicketID = item.TicketID,
                DateOfEvent = item.DateOfEvent,
                Title = item.Title,
                Location = item.Location,
                Price = item.Price,
                ArtistID = item.ArtistID

            }).ToList();
        }

        public JsonResult GetTickets(int eventId)
        {
            var myTickets = LoadAllTickets();

            var ticketList = from e in myTickets
                             select new
                             {
                                 id = e.TicketID,
                                 price = e.Price,
                                 title = e.Title,
                                 artistID = e.ArtistID,
                                 //artistName = artistRepository.GetArtists(e.ArtistID).Nickname,
                                 date = FormatIso8601(new DateTimeOffset(e.DateOfEvent.Year,e.DateOfEvent.Month,e.DateOfEvent.Day,e.DateOfEvent.Hour,e.DateOfEvent.Minute,e.DateOfEvent.Second, TimeSpan.FromHours(+2))),
                                 location = e.Location,
                                 eventID = e.EventID,
                                 beginDate = _repository.GetEvent(eventId).EventStartDateTime.Value.ToShortDateString(),
                                 finishDate = _repository.GetEvent(eventId).EventEndDateTime.Value.ToShortDateString()
                             };
            var rows = ticketList.ToArray();
            return Json(rows.Where(x => x.eventID == eventId), JsonRequestBehavior.AllowGet);
        }

        public static string FormatIso8601(DateTimeOffset dto)
        {
            string format = dto.Offset == TimeSpan.Zero
                ? "yyyy-MM-ddTHH:mm:ss.fffZ"
                : "yyyy-MM-ddTHH:mm:ss.fffzzz";

            return dto.ToString(format, CultureInfo.InvariantCulture);
        }

        [HttpPost]
        public ActionResult TicketEdit(TicketViewModel theTicket)
        {
            var model = Mapper.Map<TicketViewModel, Ticket>(theTicket);
            DateTime readyDate = theTicket.DateOfEvent + theTicket.TimeOfEvent;
            model.DateOfEvent = readyDate;
            if (ModelState.IsValid)
            {
                _ticketRepository.SaveTicket(model);
            }
            return RedirectToAction("Edit","Admin", new { eventId = model.EventID });
        }

        public PartialViewResult AddTicketPartialView(int eventId)
        {
            return PartialView("Shared/TicketEditSummary", new TicketViewModel() { EventID = eventId, ArtistList = _artistRepository.ArtistsForDropList });
        }

        //ARTYSCI
        public ViewResult AddArtist()
        {
            return View("ArtistEdit", new ArtistViewModel()
            {
                CategoriesForDropList = _catRepo.CategoriesForDropList,
            });
        }

        public ViewResult ArtistEdit(int artistId)
        {

            var theArtist = _artistRepository.Artists.FirstOrDefault(x => x.ArtistID == artistId);
            var viewModel = Mapper.Map<Artists, ArtistViewModel>(theArtist);

            viewModel.CategoriesForDropList = _catRepo.CategoriesForDropList;
      
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ArtistEdit(ArtistViewModel artist, HttpPostedFileBase image = null)
        {
            Artists model = Mapper.Map<ArtistViewModel, Artists>(artist);
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.ImageMimeType = image.ContentType;
                    model.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(model.ImageData, 0, image.ContentLength);
                }
                _artistRepository.SaveArtist(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ArtistDelete(int artistId)
        {
            Artists deletedArtist = _artistRepository.DeleteArtist(artistId);

            return RedirectToAction("Index");
        }

        //KATEGORIE
        public ActionResult GetSubcategory(int id)
        {
            return Json(PopulateSubCategory(id), JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> PopulateSubCategory(int id)
        {
            var sub = _catRepo.SubCategories.Where(x => x.EventCategoryID == id);
            var selectListItems = sub.Select(x => new SelectListItem
            {
                Value = x.EventSubCategoryID.ToString(),
                Text = x.EventSubCategoryName
            });

            return selectListItems;
        }

        public string GetCategory(int categoryId)
        {
            return _catRepo.GetCategory(categoryId);
        }

        public FileContentResult GetImage(int artistId)
        {
            var theArtist = _artistRepository.GetArtists(artistId);
            return theArtist != null ? File(theArtist.ImageData, theArtist.ImageMimeType) : null;
        }

       
    }
}