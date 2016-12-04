namespace TicketHunter.Domain.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [JsonObject(IsReference = true)]
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

        public string PublicKey { get; set; }

        public string EventKey { get; set; }
        public string ChartKey { get; set; }

        [JsonIgnore]
        public virtual Events Events { get; set; }
    }
}
