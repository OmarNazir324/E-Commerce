using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Customer:CommonEntity
    {
        [EmailAddress]
        public String email { get;set;}
        [Phone]
        public String PhoneNumber {  get;set;}
        
        public String Address { get;set;} 
        public int? OrdersCount => Orders.Count;
        public ICollection<Order> Orders { get;set;}
    }
}
