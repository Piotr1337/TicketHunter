using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Models
{
    public class TicketViewModel
    {
        [Key]
        public int TicketID { get; set; }

        public int EventID { get; set; }

        [DisplayName("Artysta/Zespół")]
        public int ArtistID { get; set; }

        [DisplayName("Data wydarzenia")]
        public DateTime DateOfEvent { get; set; }

        [DisplayName("Godzina wydarzenia")]
        public TimeSpan TimeOfEvent { get; set; }

        [DisplayName("Miejscowość")]
        public string Location { get; set; }

        [DisplayName("Cena biletu")]
        public decimal Price { get; set; }

        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [DisplayName("Klucz sali")]
        public string PublicKey { get; set; }
        [DisplayName("Klucz wydarzenia")]
        public string EventKey { get; set; }

        public IEnumerable<SelectListItem> ArtistList { get; set; }

        public virtual Events Events { get; set; }
    }
}