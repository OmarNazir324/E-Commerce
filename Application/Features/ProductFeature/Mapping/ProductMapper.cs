using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using AutoMapper;
namespace Application.Features.Product.Mapping
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Domain.Entities.Product, ProductDTO>().ReverseMap();
            CreateMap<Domain.Entities.Product, CreateProductDTO>().ReverseMap();
            CreateMap<Domain.Entities.Product, UpdateProductDTO>().ReverseMap();
        }
    }
}
