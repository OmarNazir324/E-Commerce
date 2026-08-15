using Application.Features.CommonDTO;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomerFeature.DTOs;

public class CreateCustomerDto : CreateCommnDto
{
    public String email { get; set; }
    [Phone]
    public String PhoneNumber { get; set; }

    public String Address { get; set; }

}
