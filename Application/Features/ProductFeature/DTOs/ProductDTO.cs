
using Application.Features.CommonDTO;

namespace Application.Features.Product.DTOs;

public class ProductDto:ViewCommonDto
{
    public decimal Price { get; set; }
    public String CategoryName { get; set; }
    public int Stock { get; set; }

}
