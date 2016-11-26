using System.Collections.Generic;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Models
{
    public class EventsViewModel
    {
        public IEnumerable<Events> Events { get; set; }  
        public Categories CurrentCategory { get; set; }
        public Events Event { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; } 
        public IEnumerable<Artists> Artists { get; set; } 
    }
}