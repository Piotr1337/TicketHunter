using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketHunter.Models
{
    public class SearchViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public int Id { get; set; }
    }
}