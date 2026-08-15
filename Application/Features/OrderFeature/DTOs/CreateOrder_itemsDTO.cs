using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class CreateOrder_itemsDTO : CreateCommnDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
