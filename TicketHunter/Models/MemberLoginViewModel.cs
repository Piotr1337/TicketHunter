using System.ComponentModel.DataAnnotations;

namespace TicketHunter.Models
{
    public class MemberLoginViewModel
    {
        [Required(ErrorMessage = "Proszę podać email")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Proszę podać hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}