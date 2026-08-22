using Application.CrudServiceGeneric;
using Application.Features.Product.DTOs;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.DTOs;
using AutoMapper;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.Features.ProductFeature.Service
{
    public class ProductService : MainServiceCrud<CreateProductDto, UpdateProductDto, Domain.Entities.Product>, IProductService
    {
        private readonly IGenericRepository<Domain.Entities.Product> _repo;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IGenericRepository<Domain.Entities.Product> repo, IUnitOfWork uow)
            : base(repo, mapper, uow)
        {
            this._mapper = mapper;
            this._repo = repo;
        }


        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var Products = await _repo.GetALLAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(Products);
        }
        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _repo.GetByIdAsync(productId);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
