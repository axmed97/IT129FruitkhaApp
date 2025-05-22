using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public string Description { get; set; }
    public int Count { get; set; }
    public string? PhotoUrl { get; set; }
    public int CategoryId { get; set; }
    // Navigation Property
    public Category? Category { get; set; }
}
