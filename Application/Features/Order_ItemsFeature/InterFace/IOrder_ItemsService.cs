using Application.CrudServiceGeneric;
using Application.Features.Order_ItemsFeature.DTOs;

namespace Application.Features.Order_ItemsFeature.InterFace;

public interface IOrder_ItemsService:ImainServiceCRUD<CreateOrder_itemsDTO,UpdateOrder_ItemsDTO>
{
    Task<IEnumerable<Order_itemsDTO>> GetAll();
    Task<Order_itemsDTO> GetById(int id);
}
