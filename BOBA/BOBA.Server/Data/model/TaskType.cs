using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data.model;

public class TaskType
{
    [Key]
    public string Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [ForeignKey("StarterTeam")]
    public string? StarterTeamId { get; set; }

    public virtual Team? StarterTeam { get; set; }

    public virtual ICollection<TaskDocType> DocTypes { get; set; } = new List<TaskDocType>();
}
