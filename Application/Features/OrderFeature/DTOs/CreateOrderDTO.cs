using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class CreateOrderDto : CreateCommnDto
{
    public int CustomerId { get; set; }
    public ICollection<CreateOrder_itemsDto> CreateOrder_Items { get; set; }
}
