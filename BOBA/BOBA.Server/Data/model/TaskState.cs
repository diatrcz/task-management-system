using BOBA.Server.Data.implementation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task = BOBA.Server.Data.implementation.Task;

namespace BOBA.Server.Data.model
{
    public class TaskState
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsFinal { get; set; }

        //Task
        [InverseProperty("CurrentState")]
        public virtual ICollection<Task> CurrentStateTasks { get; set; } = new List<Task>();

        //Taskflow
        [InverseProperty("CurrentState")]
        public virtual ICollection<TaskFlow> CurrentStateTaskflows { get; set; } = new List<TaskFlow>();
    }
}
