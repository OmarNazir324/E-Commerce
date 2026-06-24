using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using Application.Features.CustomerFeature.InterFaces;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.Features.CustomerFeature.Service;
public class CustomerService:MainServiceCrud<CreateCustomerDTO,UpdateCustomerDTO,Customer>,ICustomerService
{
    private readonly IMainInterFace<Customer> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public CustomerService(IMainInterFace<Customer> repo, IMapper mapper,IUnitOfWork uow)
        :base(repo,mapper,uow)
    {
        _repo = repo;
        _mapper = mapper;
        _uow = uow;
    }
    public async Task<IEnumerable<CustomerDTO>> GetAll()
    {
        var result = await _repo.GetAllAsync(c=> c.Orders);
        return _mapper.Map<IEnumerable<CustomerDTO>>(result);
    }
    public async Task<CustomerDTO> GetById(int id)
    {
        var result =await _repo.GetByID(id);
        return _mapper.Map<CustomerDTO>(result);
    }
   
}
