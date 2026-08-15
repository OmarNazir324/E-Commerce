using Application.CrudServiceGeneric;
using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
namespace Application.Features.Product.Interfaces;

public interface IProductService: ImainServiceCRUD<CreateProductDto,UpdateProductDto,Domain.Entities.Product>
{
    Task<IEnumerable<ProductDto>> GetProducts();
    Task<ProductDto> GetProductById(int productId);
}
