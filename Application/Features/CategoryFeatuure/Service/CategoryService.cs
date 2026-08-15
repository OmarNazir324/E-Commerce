using Application.CrudServiceGeneric;
using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.CategoryFeatuure.Interfaces;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.Features.CategoryFeatuure.Service;

public class CategoryService : MainServiceCrud<CreateCategoryDto, UpdateCategoryDto, Category>, ICategoryService
{
    private readonly IMainInterFace<Category> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public CategoryService(IMainInterFace<Category> repo, IMapper mapper, IUnitOfWork uow)
        : base(repo, mapper, uow)
    {
        this._mapper = mapper;
        this._repo = repo;
        this._uow = uow;
    }
    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        var categories = await _repo.GetAllAsync(categories => categories.ParentCategory);
        var result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return result;
    }
    public async Task<CategoryDto?> GetByid(int id)
    {
        var category = await _repo.FindAsync(c => c.Id == id, cc => cc.ParentCategory);
        if (category is null) return null;
        var result = _mapper.Map<CategoryDto>(category.First());
        return result;
    }
}
