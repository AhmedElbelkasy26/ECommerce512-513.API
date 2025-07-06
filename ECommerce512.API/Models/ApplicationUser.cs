using Microsoft.AspNetCore.Identity;

namespace ECommerce512.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public int Age { get; set; }
    }
}
