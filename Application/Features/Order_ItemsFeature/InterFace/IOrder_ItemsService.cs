using Application.CrudServiceGeneric;
using Application.Features.Order_ItemsFeature.DTOs;
using Domain.Entities;

namespace Application.Features.Order_ItemsFeature.InterFace;

public interface IOrder_ItemsService:ImainServiceCRUD<CreateOrder_itemsDTO,UpdateOrder_ItemsDTO,Order_items>
{
    Task<IEnumerable<Order_itemsDTO>> GetAll();
    Task<Order_itemsDTO> GetById(int id);
}
