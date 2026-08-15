using Application.Features.CommonDTO;

namespace Application.Features.ProductFeature.DTOs;

public class UpdateProductDto : ViewCommonDto
{

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }

}
