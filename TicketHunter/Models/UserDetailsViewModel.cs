using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketHunter.Models
{
    public class UserDetailsViewModel
    {
       public UserDataViewModel UserDataModel { get; set; }
       public UserAddressViewModel UserAddressModel { get; set; }
    }
}