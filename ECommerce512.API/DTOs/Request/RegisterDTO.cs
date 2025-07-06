using System.ComponentModel.DataAnnotations;

namespace ECommerce512.API.DTOs.Request
{
    public class RegisterDTO
    {

        [Required]
        [Length(5, 50)]
        public string UserName { get; set; }

        [Required]
        [Length(6, 50)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Length(6, 50)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "ConfirmPassword")]
        public string conPassword { get; set; }
    }
}
