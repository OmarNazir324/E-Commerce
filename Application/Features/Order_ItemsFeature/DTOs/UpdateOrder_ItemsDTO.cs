using Application.Features.CommonDTO;

namespace Application.Features.Order_ItemsFeature.DTOs;

public class UpdateOrder_ItemsDTO:CreateCommnDTO
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
