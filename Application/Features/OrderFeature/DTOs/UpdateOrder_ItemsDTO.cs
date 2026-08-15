using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class UpdateOrder_ItemsDto:CreateCommnDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
