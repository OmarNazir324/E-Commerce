using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using Application.Features.CustomerFeature.InterFaces;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.Features.CustomerFeature.Service;

public class CustomerService : MainServiceCrud<CreateCustomerDto, UpdateCustomerDto, Customer>, ICustomerService
{
    private readonly IGenericRepository<Customer> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public CustomerService(IGenericRepository<Customer> repo, IMapper mapper, IUnitOfWork uow)
        : base(repo, mapper, uow)
    {
        _repo = repo;
        _mapper = mapper;
        _uow = uow;
    }
    public async Task<IEnumerable<CustomerDto>> GetAll()
    {
        var result = await _repo.GetAllAsync(c => c.Orders);
        return _mapper.Map<IEnumerable<CustomerDto>>(result);
    }
    public async Task<CustomerDto> GetById(int id)
    {
        var result = await _repo.GetByID(id);
        return _mapper.Map<CustomerDto>(result);
    }

}
