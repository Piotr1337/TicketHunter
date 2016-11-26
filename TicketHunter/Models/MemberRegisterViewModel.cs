using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketHunter.Models
{
    public class MemberRegisterViewModel
    {
        [Required(ErrorMessage = "Proszę podać email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę podać Hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required]
        public string RepeatPassword { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}