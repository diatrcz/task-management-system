using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data;

public class FormField
{
    [Key]
    public string Id { get; set; }

    [Required]
    [ForeignKey("TaskField")]
    public string ModelId { get; set; }
    public TaskField TaskField { get; set; }

    [Required]
    [ForeignKey("Task")]
    public string TaskId { get; set; }
    public  Task Task { get; set; }

    [StringLength(500)]
    public string Value { get; set; }

    [StringLength(100)]
    public string Validation { get; set; }

    [Required]
    public string ModifierId { get; set; }

}
