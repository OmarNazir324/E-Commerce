using Application.Features.CommonDTO;

namespace Application.Features.OrderFeature.DTOs;

public class UpdateOrderDto : CreateCommnDto
{
    public int CustomerId { get; set; }
    public bool IsRefunded { get; set; }

    public bool IsDeleted { get; set; }
    public ICollection<UpdateOrder_ItemsDto> UpdateOrder_Items { get; set; }

}
