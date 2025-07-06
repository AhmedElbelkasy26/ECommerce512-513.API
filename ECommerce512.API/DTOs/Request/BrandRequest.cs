namespace ECommerce512.API.DTOs.Request
{
    public class BrandRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
