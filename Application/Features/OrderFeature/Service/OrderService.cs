using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.InterFace;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Features.OrderFeature.Service;

public class OrderService:MainServiceCrud<CreateOrderDTO,UpdateOrderDTO,Order> ,IOrderService
{
    private readonly IMainInterFace<Domain.Entities.Order> _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public OrderService(IMainInterFace<Domain.Entities.Order> repo,IMapper mapper,IUnitOfWork uow)
        :base(repo,mapper,uow)
    {
        _repo = repo;
        _mapper = mapper;
        _uow= uow;
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
    public override async Task Create(CreateOrderDTO create)
    {
        try
        {
            var result =_mapper.Map<Order>(create);
            await _uow.BeginTransactionAsync();
            await _repo.Create(result);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _uow.RollbackTransactionAsync();
            throw ex;
        }
    }
    
}
