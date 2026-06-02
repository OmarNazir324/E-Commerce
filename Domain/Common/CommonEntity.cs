using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public class CommonEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { set; get; } = String.Empty;
    public string? Description { set; get; }
    public DateTime CreatedAt { set; get; } 
    public DateTime? UpdatedAt { set; get; } 
    public int? User_Code { get; set; }

}
