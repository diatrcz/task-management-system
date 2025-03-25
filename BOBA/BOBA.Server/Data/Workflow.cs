using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace BOBA.Server.Data
{
    public class Workflow
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("TaskType")]
        public string TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        [Required]
        [ForeignKey("CurrentState")]
        public string CurrentStateId { get; set; }
        public TaskStatus CurrentState { get; set; }

        [Required]
        [ForeignKey("NextState")]
        public string NextStateId { get; set; }
        public TaskStatus NextState { get; set; }

        [ForeignKey("EditRole")]
        public string? EditRoleId { get; set; }
        public virtual Team? EditRole { get; set; }

        public ICollection<Team> ReadOnlyRole { get; set; } = new List<Team>();
        public bool Approved { get; set; }
    }
}
