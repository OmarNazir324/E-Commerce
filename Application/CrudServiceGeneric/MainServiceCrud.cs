using AutoMapper;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.CrudServiceGeneric;

public class MainServiceCrud<CreateDTO, UpdateDTO, MainEntity> : ImainServiceCRUD<CreateDTO, UpdateDTO, MainEntity>
   where CreateDTO : class where UpdateDTO : class where MainEntity : class
{
    private readonly IGenericRepository<MainEntity> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitofwork;
    public MainServiceCrud(IGenericRepository<MainEntity> repo, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _mapper = mapper;
        _unitofwork = unitOfWork;
    }
    public virtual async Task<(bool Status, String MSG, MainEntity? entity)> Create(CreateDTO create, params object?[] parameters)
    {
        try
        {
            var result = _mapper.Map<MainEntity>(create);
            await _repo.Create(result);
            await _unitofwork.SaveChangesAsync();
            return (true, "Success", result);
        }
        catch (Exception ex)
        {

            return (false, ex.Message, null);
        }


    }
    public virtual async Task Update(UpdateDTO update)
    {
        var result = _mapper.Map<MainEntity>(update);
        await _repo.Update(result);
        await _unitofwork.SaveChangesAsync();
    }
    public virtual async Task<(Boolean Status, String? msg)> Delete(int id, params object?[] parameters)
    {
        var result = await _repo.GetByID(id);
        if (result is null) return (false, "Can't Find This Object");
        await _repo.Delete(result);
        await _unitofwork.SaveChangesAsync();
        return (true, null);
    }
    public virtual async Task<(Boolean Status, String? msg)> Delete(MainEntity t, params object?[] parameters)
    {
        await _repo.Delete(t);
        await _unitofwork.SaveChangesAsync();
        return (true, Task.CompletedTask.ToString());
    }
}