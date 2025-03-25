using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data
{
    public class TaskStatus
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsFinal { get; set; }

        [InverseProperty("CurrentState")]
        public virtual ICollection<Workflow> CurrentStateWorkflows { get; set; } = new List<Workflow>();

        [InverseProperty("NextState")]
        public virtual ICollection<Workflow> NextStateWorkflows { get; set; } = new List<Workflow>();
    }
}
