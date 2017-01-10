namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketArtists
    {
        public int TicketArtistsID { get; set; }

        public int ArtistID { get; set; }

        public int TicketID { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
