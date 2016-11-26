using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHunter.Domain.Entities
{
    public class Countries
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
