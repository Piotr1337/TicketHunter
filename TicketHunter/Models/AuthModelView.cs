using System.Web.Mvc;

namespace TicketHunter.Models
{
    public class AuthModelView
    {
        public MemberLoginViewModel LoginModel { get; set; }
        public MemberRegisterViewModel RegisterModel { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}