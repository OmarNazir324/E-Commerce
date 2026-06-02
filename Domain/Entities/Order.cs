namespace Domain.Entities;

public class Order : Common.CommonEntity
{   
    public decimal? TotalPrice
    {
        get; set;
    }
    public int? TotalQuantity { set; get; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public ICollection<Order_items>  Order_Items { get; set; }
    
    public void clac_TotalPrice()
    {
        this.TotalPrice = (Order_Items == null ? 0 : Order_Items.Sum(p => p.Product.Price) * Order_Items.Sum(oi => oi.Quantity));
        this.TotalQuantity = Order_Items!.Sum(oi => oi.Quantity);
    }

}
