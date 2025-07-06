using System.ComponentModel.DataAnnotations;

namespace ECommerce512.API.DTOs.Response
{
    public class BrandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Note { get; set; }
        public bool Status { get; set; }
    }
}
