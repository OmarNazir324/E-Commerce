using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using Domain.Entities;

namespace Application.Features.CustomerFeature.InterFaces;

public interface ICustomerService : ImainServiceCRUD<CreateCustomerDTO, UpdateCustomerDTO, Customer>
{
    Task<CustomerDTO> GetById(int id);
    Task<IEnumerable<CustomerDTO>> GetAll();
}
