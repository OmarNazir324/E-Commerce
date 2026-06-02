using Application.Features.CommonDTO;

namespace Application.Features.Order_ItemsFeature.DTOs;

public class CreateOrder_itemsDTO : CreateCommnDTO
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
