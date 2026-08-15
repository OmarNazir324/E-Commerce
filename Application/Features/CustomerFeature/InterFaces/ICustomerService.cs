using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using Domain.Entities;

namespace Application.Features.CustomerFeature.InterFaces;

public interface ICustomerService : ImainServiceCRUD<CreateCustomerDto, UpdateCustomerDto, Customer>
{
    Task<CustomerDto> GetById(int id);
    Task<IEnumerable<CustomerDto>> GetAll();
}
