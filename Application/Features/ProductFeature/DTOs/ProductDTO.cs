
using Application.Features.CommonDTO;

namespace Application.Features.Product.DTOs
{
    public class ProductDTO:ViewCommonDTO
    {
        public decimal Price { get; set; }
        public String CategoryName { get; set; }
        
    }
}
