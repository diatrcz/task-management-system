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
        public List<string> NextState
        {
            get => JsonSerializer.Deserialize<List<string>>(NextStateJson) ?? new();
            set => NextStateJson = JsonSerializer.Serialize(value);
        }

        [ForeignKey("EditRole")]
        public string? EditRoleId { get; set; }
        public virtual Team? EditRole { get; set; }

        public ICollection<Team> ReadOnlyRole { get; set; } = new List<Team>();
    }
}
