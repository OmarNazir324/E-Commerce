using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.CategoryFeatuure.Interfaces;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatuure.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly IMainInterFace<Category> _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public CategoryService(IMainInterFace<Category> repo,IMapper mapper,IUnitOfWork uow)
        {
            this._mapper = mapper;
            this._repo = repo;
            this._uow = uow;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _repo.GetAllAsync(categories=> categories.ParentCategory);
            var result=_mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return result;
        }
        public async Task<CategoryDTO?> GetByid(int id)
        {
            var category = await _repo.FindAsync(c=> c.Id==id,cc=> cc.ParentCategory);
            if (category is null) return null;
            var result = _mapper.Map<CategoryDTO>(category.First());
            return result;
        }
            
        public async Task Create(CreateCategoryDTO createCategoryDTO)
        {

            var category = _mapper.Map<Category>(createCategoryDTO);
            await _repo.Create(category);
            await _uow.BeginTransactionAsync();
        }
        public async Task Update(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = _mapper.Map<Category>(updateCategoryDTO);
            await _repo.Update(category);
            await _uow.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var category=await _repo.GetByID(id);
            await _repo.Delete(category);
            await _uow.SaveChangesAsync();
        }
    }
}
