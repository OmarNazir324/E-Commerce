using Application.Features.CommonDTO;
using Application.Features.Order_ItemsFeature.DTOs;
using System.Diagnostics.Contracts;

namespace Application.Features.OrderFeature.DTOs;

public class OrderDTO : ViewCommonDTO
{
    public decimal? TotalPrice { get; set; }
    public int? TotalQuantity { get; set; }
    public String Customer_Name { get; set; }
    public ICollection<Order_itemsDTO> Order_ItemsDTOs { get; set; }

}
