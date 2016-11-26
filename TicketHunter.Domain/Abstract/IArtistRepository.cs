using System.Collections.Generic;
using System.Web.Mvc;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Abstract
{
    public interface IArtistRepository
    {
        IEnumerable<Artists> Artists { get; }

        IEnumerable<SelectListItem> ArtistsForDropList { get; }

        void SaveArtist(Artists theArtist);

        Artists GetArtists(int artistId);

        Artists DeleteArtist(int artistId);


    }
}
