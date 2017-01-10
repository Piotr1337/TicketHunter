using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Abstract
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> Tickets { get; }

        IEnumerable<TicketArtists> TicketArtists { get; }

        int SaveTicket(Ticket theTicket);

        void SaveTicketArtists(TicketArtists theTicketArtists);

        Ticket DeleteTicket(int ticketId);

        Ticket GetTicket(int ticketId);
    }
}
