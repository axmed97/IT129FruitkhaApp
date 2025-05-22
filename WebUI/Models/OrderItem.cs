using WebUI.Enums;

namespace WebUI.Models;

public class OrderItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
