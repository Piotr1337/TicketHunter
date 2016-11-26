namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        public int TicketID { get; set; }

        public int EventID { get; set; }

        public DateTime DateOfEvent { get; set; }

        [Required]
        public string Location { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Title { get; set; }

        public int? OrderID { get; set; }

        public int ArtistID { get; set; }

        public virtual Events Events { get; set; }
    }
}
