using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using Application.Features.CustomerFeature.InterFaces;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Repositories.Generic;

namespace Application.Features.CustomerFeature.Service;
public class CustomerService:MainServiceCrud<CreateCustomerDTO,UpdateCustomerDTO,Customer>,ICustomerService
{
    private readonly IMainInterFace<Customer> _repo;
    private readonly IMapper _mapper;
    public CustomerService(IMainInterFace<Customer> repo, IMapper mapper)
        :base(repo,mapper)
    {
        _repo = repo;
        _mapper = mapper;
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
