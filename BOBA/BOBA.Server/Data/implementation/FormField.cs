using BOBA.Server.Data.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data.implementation;

public class FormField
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

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

    [Required]
    [ForeignKey("Modifier")]
    public string ModifierId { get; set; }
    public User Modifier { get; set;  }
}
