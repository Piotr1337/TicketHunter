namespace TicketHunter.Domain.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            TicketArtists = new HashSet<TicketArtists>();
        }

        public int TicketID { get; set; }

        public int EventID { get; set; }

        public DateTime DateOfEvent { get; set; }

        [Required]
        public string Location { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Title { get; set; }

        public int? OrderID { get; set; }

        [StringLength(50)]
        public string PublicKey { get; set; }

        [StringLength(50)]
        public string EventKey { get; set; }

        [StringLength(50)]
        public string SecretKey { get; set; }

        [StringLength(50)]
        public string ChartKey { get; set; }

        [JsonIgnore]
        public virtual Events Events { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TicketArtists> TicketArtists { get; set; }
    }
}
