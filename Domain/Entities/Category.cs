

namespace Domain.Entities;

public partial class Category:Common.CommonEntity
{
    public int? Main_Category { get; set; }
    public Category ParentCategory { get; set; }

    public ICollection<Product> Products { get; set; }
    public ICollection<Category> SubCategories { get; set; }
}
