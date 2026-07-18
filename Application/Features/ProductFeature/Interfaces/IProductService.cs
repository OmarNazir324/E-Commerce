using Application.CrudServiceGeneric;
using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using Domain.Entities;
namespace Application.Features.Product.Interfaces
{
    public interface IProductService: ImainServiceCRUD<CreateProductDTO,UpdateProductDTO,Domain.Entities.Product>
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int productId);
    }
}
