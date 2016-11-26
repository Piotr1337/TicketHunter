using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Models
{
    public class UserAddressViewModel
    {
        public int UserAddressID { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string FlatNumber { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string City { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string Country { get; set; }

        public IEnumerable<UserAddress> UserAddresses { get; set; }

        public IEnumerable<Countries> Countries { get; set; }

        public IEnumerable<SelectListItem> CountiresDropDown { get; set; }
    }
}