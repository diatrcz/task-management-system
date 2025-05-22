using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Data;

public class FormField
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string ModelId { get; set; }

    [Required]
    public string TaskId { get; set; }

    [StringLength(500)]
    public string Value { get; set; }

    [StringLength(100)]
    public string Validation { get; set; }

    [Required]
    public string ModifierId { get; set; }

}
