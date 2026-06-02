using Domain.Common;
namespace Domain.Entities;

public class Order_items : CommonEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Quantity * Product.Price;
    public Order Order { get; set; }
    public Product Product { get; set; }
}
