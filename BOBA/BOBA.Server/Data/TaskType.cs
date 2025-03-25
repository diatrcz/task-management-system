using System.ComponentModel.DataAnnotations;

namespace BOBA.Server.Data
{
    public class TaskType
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        /*public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();*/

    }
}
