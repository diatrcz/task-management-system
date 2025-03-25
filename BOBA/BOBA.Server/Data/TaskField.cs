using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Data;

public class TaskField
{
    [Key]
    public string Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
    [StringLength(100)]
    public string Validation { get; set; }
}
