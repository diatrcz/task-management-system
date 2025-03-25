using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data
{
    //List of tasks users have started
    public class Task
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("TaskType")]
        public string TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        [Required]
        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public User Creator { get; set; }

        [Required]
        [ForeignKey("Workflow")]
        public string WorkflowId { get; set; }
        public Workflow Workflow { get; set; }

        [ForeignKey("Assignee")]
        public string AssigneeId { get; set; }
        public User Assignee { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
