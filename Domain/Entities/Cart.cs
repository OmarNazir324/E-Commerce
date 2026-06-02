using Domain.Common;

namespace Domain.Entities;

public class Cart : CommonEntity
{
    public bool? IsRefunded { get; set; }

    public bool? IsDeleted { get; set; }
    public int OrderId { get; set; }
    public ICollection<Order> Orders { get; set; }
}
