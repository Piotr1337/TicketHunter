using System.Collections.Generic;
using System.Threading.Tasks;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Abstract
{
    public interface IEventRepository
    {
        IEnumerable<Events> Events { get; }

        Task<IEnumerable<Events>> EventsAsync(int? categoryId, int? subcategoryId);


        Events GetEvent(int? eventId);

        Task<Events> GetEventAsync(int? eventId);


        void SaveEvent(Events theEvent);

        Events DeleteEvent(int eventId);
    }
}
