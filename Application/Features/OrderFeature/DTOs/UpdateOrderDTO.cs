using Application.Features.CommonDTO;
using Application.Features.Order_ItemsFeature.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeature.DTOs
{
    public class UpdateOrderDTO :CreateCommnDTO
    {
        public int CustomerId { get; set; }
        public bool IsRefunded { get; set; }
        
        public bool IsDeleted { get; set; }
        public ICollection<UpdateOrder_ItemsDTO> Order_Items { get; set; }

    }
}
