using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.InterFace;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Repositories.Generic;

namespace Application.Features.OrderFeature.Service;

public class OrderService:MainServiceCrud<CreateOrderDTO,UpdateOrderDTO,Order> ,IOrderService
{
    private readonly IMainInterFace<Domain.Entities.Order> _repo;
    private readonly IMapper _mapper;
    public OrderService(IMainInterFace<Domain.Entities.Order> repo,IMapper mapper)
        :base(repo,mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<IEnumerable<OrderDTO>> GetAll()
    {
        var result =await _repo.GetAllAsync(x=> x.Customer);
        return _mapper.Map<IEnumerable<OrderDTO>>(result);
    }
    public async Task<OrderDTO> GetById(int id)
    {
        var result= await _repo.GetByID(id);
        return _mapper.Map<OrderDTO>(result);
    }
    
}
