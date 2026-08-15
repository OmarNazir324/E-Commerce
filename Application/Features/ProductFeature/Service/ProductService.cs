using Application.CrudServiceGeneric;
using Application.Features.Product.DTOs;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.DTOs;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using System.Diagnostics;

namespace Application.Features.ProductFeature.Service
{
    public class ProductService: MainServiceCrud<CreateProductDto,UpdateProductDto,Domain.Entities.Product> , IProductService
    {
        private readonly IMainInterFace<Domain.Entities.Product> _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public ProductService(IMapper mapper, IMainInterFace<Domain.Entities.Product> repo, IUnitOfWork uow)
            :base(repo,mapper,uow)
        {
            this._mapper = mapper;
            this._repo = repo;
            this._uow = uow;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var Products = await _repo.GetALL();
            var result = _mapper.Map<IEnumerable<ProductDto>>(Products);
            if (result == null) return null;
            return result;
        }
        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _repo.GetByID(productId);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
