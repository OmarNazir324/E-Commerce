using Application.Features.Product.DTOs;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.DTOs;
using AutoMapper;
using InfraStructure.Repositories.Generic;

namespace Application.Features.ProductFeature.Service
{
    public class ProductService : IProductService
    {
         private readonly IMainInterFace<Domain.Entities.Product> _repo;
        private readonly IMapper mapper;
        public ProductService(IMapper _mapper,IMainInterFace<Domain.Entities.Product> repo)
        {
            this.mapper = _mapper;
            this._repo = repo;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var Products = await _repo.GetALL();
            var result = mapper.Map<IEnumerable<ProductDTO>>(Products);
            if (result == null) return null;
            return result;
        }
        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await _repo.GetByID(productId);
            var result = mapper.Map<ProductDTO>(product);
            if (result == null) return null;
            return result;
        }
        public async Task Create(CreateProductDTO createProductDTO)
        {
            var originalProduct = mapper.Map<Domain.Entities.Product>(createProductDTO);
            await _repo.Create(originalProduct);
        }
        public async Task Update(UpdateProductDTO updateProductDTO)
        {
            var originalProduct = mapper.Map<Domain.Entities.Product>(updateProductDTO);
            await _repo.Update(originalProduct);
        }
        public async Task Delete(int id)
        {
            var rseult = await _repo.GetByID(id);
            await _repo.Delete(rseult);
        }

    }
}
