using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class OrderDTO : ViewCommonDTO
{
    public decimal? TotalPrice { get; set; }
    public int? TotalQuantity { get; set; }
    public String Customer_Name { get; set; }

}
