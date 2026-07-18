using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;
using Domain.Entities;

namespace Application.Features.OrderFeature.InterFace;

public interface IOrderService:ImainServiceCRUD<CreateOrderDTO,UpdateOrderDTO,Order>
{
    Task<OrderDTO> GetById(int id);
    Task<IEnumerable<OrderDTO>> GetAll();
}
