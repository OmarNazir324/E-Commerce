using Application.Features.CommonDTO;

namespace Application.Features.ProductFeature.DTOs;

public class CreateProductDto : CreateCommnDto
{
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }


}
