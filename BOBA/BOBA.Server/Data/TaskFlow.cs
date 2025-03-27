using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text.Json;

namespace BOBA.Server.Data
{
    public class Taskflow
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
        public TaskState CurrentState { get; set; }

        
        public string NextStateJson { get; set; } = "[]";

        [NotMapped]
        public List<NextStateItem> NextState
        {
            get => JsonSerializer.Deserialize<List<NextStateItem>>(NextStateJson) ?? new List<NextStateItem>();
            set => NextStateJson = JsonSerializer.Serialize(value);
        }

        [ForeignKey("EditRole")]
        public string? EditRoleId { get; set; }
        public virtual Team? EditRole { get; set; }

        public ICollection<Team> ReadOnlyRole { get; set; } = new List<Team>();
    }

    public class NextStateItem
    {
        public string ChoiceId { get; set; }
        public string NextStateId { get; set; }
    }
}
