namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Countries
    {
        [Key]
        public int CountryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }
    }
}
