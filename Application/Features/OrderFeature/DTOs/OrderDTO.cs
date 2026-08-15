using Application.Features.CommonDTO;
using System.Diagnostics.Contracts;

namespace Application.Features.OrderFeature.DTOs;

public class OrderDto : ViewCommonDto
{
    public decimal? TotalPrice { get; set; }
    public int? TotalQuantity { get; set; }
    public String Customer_Name { get; set; }
    public ICollection<Order_itemsDto> Order_ItemsDTOs { get; set; }

}
