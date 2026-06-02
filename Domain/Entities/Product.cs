
namespace Domain.Entities
{
    public class Product:Common.CommonEntity
    {
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Order_items> Items { get; set; }
    }
}
