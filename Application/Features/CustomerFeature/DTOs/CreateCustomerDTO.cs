using Application.Features.CommonDTO;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeature.DTOs
{
    public class CreateCustomerDTO:CreateCommnDTO
    {
        public String email { get; set; }
        [Phone]
        public String PhoneNumber { get; set; }

        public String Address { get; set; }
        
    }
}
