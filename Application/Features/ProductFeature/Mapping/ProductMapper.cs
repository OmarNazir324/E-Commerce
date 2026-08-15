using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using AutoMapper;
namespace Application.Features.Product.Mapping
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Domain.Entities.Product, ProductDto>().ReverseMap();
            CreateMap<Domain.Entities.Product, CreateProductDto>().ReverseMap();
            CreateMap<Domain.Entities.Product, UpdateProductDto>().ReverseMap();
        }
    }
}
