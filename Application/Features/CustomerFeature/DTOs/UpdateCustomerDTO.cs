using Application.Features.CommonDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeature.DTOs
{
    public class UpdateCustomerDTO:ViewCommonDTO
    {
        [EmailAddress]
        public String email { get; set; }
        [Phone]
        public String PhoneNumber { get; set; }

        public String Address { get; set; }
    }
}
