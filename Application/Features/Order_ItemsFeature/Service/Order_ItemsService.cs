using Application.CrudServiceGeneric;
using Application.Features.Order_ItemsFeature.DTOs;
using Application.Features.Order_ItemsFeature.InterFace;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Interfaces;

namespace Application.Features.Order_ItemsFeature.Service;

public class Order_ItemsService : MainServiceCrud<CreateOrder_itemsDTO, UpdateOrder_ItemsDTO, Order_items>, IOrder_ItemsService
{
    private readonly IOrder_itemsRepository _repo;
    private readonly IMapper _mapper;
    public Order_ItemsService(IOrder_itemsRepository repo, IMapper mapper)
        : base(repo, mapper)
    {
        {
            _repo = repo;
            _mapper = mapper;
        }
    }
    public async Task<IEnumerable<Order_itemsDTO>> GetAll()
    {
        var result = await _repo.GetAllAsync(oi=> oi.Product);
        return _mapper.Map<IEnumerable<Order_itemsDTO>>(result);
    }
    public async Task<Order_itemsDTO> GetById(int id)
    {
        var result = await _repo.FindAsync(i=> i.Id==id,p => p.Product);
        return _mapper.Map<Order_itemsDTO>(result);
    }
    public override Task Create(CreateOrder_itemsDTO create)
    {
        var res = base.Create(create);
        return res;
    }
}
