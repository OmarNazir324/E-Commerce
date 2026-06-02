using Application.Features.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductFeature.DTOs
{
    public class CreateProductDTO:CreateCommnDTO
    {
        public int Stock {  get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }


    }
}
