using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TicketHunter.Domain.Entities;


namespace TicketReservation.Models
{
    public class AdminViewModel
    {
        [Key]
        public int EventID { get; set; }

        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "Proszę wybrać kategorię")]
        public int EventCategoryID { get; set; }

        public int ArtistID { get; set; }

        [Display(Name = "Podkategoria")]
        [Required(ErrorMessage = "Proszę wybrać Podkategorię")]
        public int EventSubCategoryID { get; set; }

        public int PlaceID { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwe wydarzenia")]
        [Display(Name = "Nazwa")]
        public string EventName { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Data rozpoczęcia")]
        [Required(ErrorMessage = "Proszę podać date rozpoczęcia wydarzenia")]
        public DateTime EventStartDateTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Data zakończenia")]
        [Required(ErrorMessage = "Proszę podać date zakończenia wydarzenia")]
        public DateTime EventEndDateTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Data publikacji")]
        public DateTime PublicationDate { get; set; }

        [DataType(DataType.MultilineText), Display(Name = "Opis")]
        [Required(ErrorMessage = "Proszę podać opis")]
        [AllowHtml]
        public string OtherDetails { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        public virtual SubCategories SubCategory { get; set; }

        public virtual Categories Category { get; set; }

        public virtual Events Events { get; set; }

        public bool ShowCalendar { get; set; }

        public IEnumerable<SelectListItem> SubCategoryForDropList { get; set; }

        public IEnumerable<SelectListItem> CategoriesForDropList { get; set; }
    }
}