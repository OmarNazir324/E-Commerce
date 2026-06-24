using Application.Features.CommonDTO;
using Application.Features.Order_ItemsFeature.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeature.DTOs
{
    public class CreateOrderDTO:CreateCommnDTO
    {
        public int CustomerId { get; set; }
        public ICollection<CreateOrder_itemsDTO> Order_Items { get; set; }
    }
}
