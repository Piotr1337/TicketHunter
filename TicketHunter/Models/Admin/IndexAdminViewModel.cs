﻿using System.Collections.Generic;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Models.Admin
{
    public class IndexAdminViewModel
    {
        public IEnumerable<Events> IndexEvents { get; set; }
        public IEnumerable<Artists> IndexArtists { get; set; }
        public IEnumerable<Ticket> IndexTickets { get; set; }
        public Events Event { get; set; }
    }
}