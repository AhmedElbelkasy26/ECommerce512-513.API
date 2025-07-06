using System.ComponentModel.DataAnnotations;

namespace ECommerce512.API.DTOs.Request
{
    public class LoginDTO
    {
        [Required]
        [Display(Name = "UserName Or Email")]
        public string Account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
