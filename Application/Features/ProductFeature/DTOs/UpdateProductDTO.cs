using Application.Features.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductFeature.DTOs
{
    public class UpdateProductDTO:ViewCommonDTO
    {
       
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

    }
}
