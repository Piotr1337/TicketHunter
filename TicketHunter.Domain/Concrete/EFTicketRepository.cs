using System;
using System.Collections.Generic;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Concrete
{
    public class EFTicketRepository : ITicketRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Ticket> Tickets
        { 
            get { return context.Ticket; }
        }

        public IEnumerable<TicketArtists> TicketArtists
        {
            get { return context.TicketArtists; }
        }

        public int SaveTicket(Ticket theTicket)
        {
            if (theTicket.TicketID == 0)
            {
                context.Ticket.Add(theTicket);
            }
            else
            {
                Ticket dbEntry = context.Ticket.Find(theTicket.TicketID);
                if (dbEntry != null)
                {
                    dbEntry.DateOfEvent = theTicket.DateOfEvent;
                    dbEntry.Location = theTicket.Location;
                    dbEntry.Title = theTicket.Title;
                    dbEntry.Price = theTicket.Price;
                    dbEntry.PublicKey = "829ec0d2-42b6-481e-86d4-b23b7f8f7691";
                    dbEntry.SecretKey = "9081cdd9-d70c-43e8-87ba-41ec8778c518";
                }
            }
            context.SaveChanges();
            return theTicket.TicketID;
        }

        public Ticket DeleteTicket(int ticketId)
        {
            Ticket dbEntry = context.Ticket.Find(ticketId);
            if (dbEntry != null)
            {
                context.Ticket.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Ticket GetTicket(int ticketId)
        {
            Ticket dbEntry = context.Ticket.Find(ticketId);
            return dbEntry;
        }

        public void SaveTicketArtists(TicketArtists theTicketArtists)
        {
            if (theTicketArtists.TicketArtistsID == 0)
            {
                context.TicketArtists.Add(theTicketArtists);
            }
            else
            {
                TicketArtists dbEntry = context.TicketArtists.Find(theTicketArtists.TicketID);
                if (dbEntry != null)
                {
                    
                }
            }
            context.SaveChanges();
        }
    }
}
