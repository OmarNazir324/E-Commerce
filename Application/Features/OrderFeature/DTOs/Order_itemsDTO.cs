using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class Order_itemsDto:ViewCommonDto
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public String Product_Name { get; set; }
}
