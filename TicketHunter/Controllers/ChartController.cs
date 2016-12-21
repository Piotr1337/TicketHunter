using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketHunter.Domain.Abstract;
using TicketHunter.Models;

namespace TicketHunter.Controllers
{
    [Authorize(Roles = "User")]
    public class ChartController : Controller
    {
        ITicketRepository _ticketRepository;
        public ChartController(ITicketRepository ticketRepo)
        {
            _ticketRepository = ticketRepo;
        }
        // GET: Chart
        public ActionResult Chart(int ticketId)
        {

            ViewBag.ticketId = ticketId;
            return View(new ChartViewModel() {
                Ticket = _ticketRepository.GetTicket(ticketId)
            });
        }

        
        public ActionResult ChartInfo(int ticketId)
        {
            var ticket = _ticketRepository.GetTicket(ticketId);
            string jsonResult = JsonConvert.SerializeObject(ticket, Formatting.Indented);

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ReservingInfoSummary(ReserveInfoViewModel vm)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ReservingInfoSummaryPost(ReserveInfoViewModel vm)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult BestAvaiableSummaryPost(ReserveInfoViewModel vm)
        {
            return PartialView();
        }
    }
}