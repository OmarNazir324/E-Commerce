using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;
using Domain.Entities;

namespace Application.Features.OrderFeature.InterFace;

public interface IOrderService:ImainServiceCRUD<CreateOrderDto,UpdateOrderDto,Order>
{
    Task<OrderDto> GetById(int id);
    Task<IEnumerable<OrderDto>> GetAll();
}
