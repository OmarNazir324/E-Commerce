using Application.CrudServiceGeneric;
using Application.Features.OrderFeature.DTOs;

namespace Application.Features.OrderFeature.InterFace;

public interface IOrderService:ImainServiceCRUD<CreateOrderDTO,UpdateOrderDTO>
{
    Task<OrderDTO> GetById(int id);
    Task<IEnumerable<OrderDTO>> GetAll();
}
