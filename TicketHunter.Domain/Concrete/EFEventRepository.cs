using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;
using TicketHunter.Concrete;

namespace TicketHunter.Domain.Concrete
{
    public class EFEventRepository : IEventRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Events> Events
        {
            get
            { 
                return context.Events;
            }
        }

        public async Task<IEnumerable<Events>> EventsAsync(int? categoryId, int? subcategoryId)
        {
            if (categoryId != null)
            {
                var eventList = await Task.Run(() => context.Events.Where(x => x.EventCategoryID == categoryId));
                return eventList;
            }
            else if (subcategoryId != null)
            {
                var eventList = await Task.Run(() => context.Events.Where(x => x.EventSubCategoryID == subcategoryId));
                return eventList;
            }
            else
            {
                var eventList = await Task.Run(() => context.Events);
                return eventList;
            }
        }


        public Events GetEvent(int? eventId)
        {
            var foundEvent = context.Events.Find(eventId);
            return foundEvent;           
        }

        public async Task<Events> GetEventAsync(int? eventId)
        {
            var foundEvent = await context.Events.FindAsync(eventId);
            return foundEvent;
        }

        public void SaveEvent(Events theEvent)
        {
            Events dbEntry = context.Events.Find(theEvent.EventID);
            if (theEvent.EventID == 0)
            {
                context.Events.Add(theEvent);
            }
            else
            {
                if (dbEntry != null)
                {
                    var mapping = Mapper.Map(theEvent, dbEntry);
                    if (!string.IsNullOrEmpty(theEvent.ImageMimeType))
                    {
                        mapping.ImageData = theEvent.ImageData;
                        mapping.ImageMimeType = theEvent.ImageMimeType;
                    }
                }
            }
            if(context.SaveChanges() > 1)
            {
                LuceneSearch.AddUpdateLuceneIndex(dbEntry);
            }
        }

        public Events DeleteEvent(int eventId)
        {
            Events dbEntry = context.Events.Find(eventId);
            List<Ticket> dbListTicket = context.Ticket.Where(x => x.EventID == eventId).ToList();
            if (dbEntry != null)
            {
                context.Events.Remove(dbEntry);
                context.Ticket.RemoveRange(dbListTicket);
                context.SaveChanges();
                if (context.SaveChanges() > 1)
                {
                    LuceneSearch.ClearLuceneIndexRecord(dbEntry.EventID);
                }
            }
            return dbEntry;           
        }
    }
}
