using Application.Features.CommonDTO;

namespace Application.Features.Order_ItemsFeature.DTOs;

public class Order_itemsDTO:ViewCommonDTO
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public String Product_Name { get; set; }
}
