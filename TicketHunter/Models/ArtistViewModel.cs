using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TicketHunter.Models
{
    public class ArtistViewModel
    {

        public int ArtistID { get; set; }

        [DisplayName("Imię")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [DisplayName("Informacje o zespole/artyście")]
        [AllowHtml]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? EventID { get; set; }

        [DisplayName("Kategoria wykonawcy / zespołu")]
        public int CategoryID { get; set; }

        public bool IsArtistOrBand { get; set; }

        [DisplayName("Pseudonim artystyczny")]
        public string Nickname { get; set; }

        public bool IsBand { get; set; }

        [DisplayName("Nazwa zespołu")]
        public string BandName { get; set; }

        public string ImageMimeType { get; set; }

        public IEnumerable<SelectListItem> CategoriesForDropList { get; set; }
    }
}