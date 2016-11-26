namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Artists
    {
        [Key]
        public int ArtistID { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(50)]
        public string Nickname { get; set; }

        public bool IsBand { get; set; }

        [StringLength(50)]
        public string BandName { get; set; }

        [StringLength(50)]
        public string ImageMimeType { get; set; }
    }
}
