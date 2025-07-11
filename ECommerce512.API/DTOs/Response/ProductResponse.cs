﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ECommerce512.API.DTOs.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public bool Status { get; set; }
        public long Traffic { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
