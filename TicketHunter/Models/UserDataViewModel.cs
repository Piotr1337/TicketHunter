using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketHunter.Models
{
    public class UserDataViewModel
    {
        public int UserProfileDetailsID { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Uzupełnij Pole")]
        public string TelephoneNumber { get; set; }

        public string Id { get; set; }
    }
}