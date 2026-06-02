using Application.CrudServiceGeneric;
using Application.Features.CustomerFeature.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeature.InterFaces
{
    public interface ICustomerService:ImainServiceCRUD<CreateCustomerDTO,UpdateCustomerDTO>
    {
        Task<CustomerDTO> GetById(int id);
        Task<IEnumerable<CustomerDTO>> GetAll();
    }
}
