using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.InterFace;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.Features.OrderFeature.Service;

public class OrderService : MainServiceCrud<CreateOrderDto, UpdateOrderDto, Order>, IOrderService
{
    private readonly IGenericRepository<Domain.Entities.Order> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public OrderService(IGenericRepository<Domain.Entities.Order> repo, IMapper mapper, IUnitOfWork uow)
        : base(repo, mapper, uow)
    {
        _repo = repo;
        _mapper = mapper;
        _uow = uow;
    }
    public async Task<IEnumerable<OrderDto>> GetAll()
    {
        var result = await _repo.GetAllAsync(x => x.Customer, x => x.Order_Items);
        return _mapper.Map<IEnumerable<OrderDto>>(result);
    }
    public async Task<OrderDto> GetById(int id)
    {
        var result = await _repo.FindAsync(x => x.Id == id, x => x.Order_Items);
        return _mapper.Map<OrderDto>(result.FirstOrDefault());
    }
    public async override Task<(bool Status, string MSG, Order? entity)> Create(CreateOrderDto create, params object?[] parameters)
    {
        try
        {
            var result = _mapper.Map<Order>(create);
            await _uow.BeginTransactionAsync();
            await _repo.Create(result);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();
            return (true, String.Empty, null);
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();
            return (false, ex.Message, null);
        }
    }

}
