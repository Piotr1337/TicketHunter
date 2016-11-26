using System.Collections.Generic;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Models
{
    public class NavbarViewModel
    {
        public IEnumerable<Categories> Categories { get; set; }
        public IEnumerable<SubCategories> SubCategories { get; set; }
        public MemberLoginViewModel LoginModel { get; set; }
        public string SearchText { get; set; }
    }
}