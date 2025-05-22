using BOBA.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text.Json;

namespace BOBA.Server.Data
{
    public class TaskFlow
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("TaskType")]
        public string? TaskTypeId { get; set; }

        public TaskType? TaskType { get; set; }

        [Required]
        [ForeignKey("CurrentState")]
        public string CurrentStateId { get; set; }

        public TaskState CurrentState { get; set; }
        
        public string NextStateJson
        {
            get { return JsonSerializer.Serialize(NextState); }
            set {
                if (string.IsNullOrEmpty(value))
                    NextState = new List<NextStateItem>();
                else
                    NextState = JsonSerializer.Deserialize<List<NextStateItem>>(value); 
            }
        }

        [NotMapped]
        public List<NextStateItem> NextState { get; set; }

        [ForeignKey("EditRole")]
        public string? EditRoleId { get; set; }

        public virtual Team? EditRole { get; set; }

        public ICollection<Team> ReadOnlyRole { get; set; } = new List<Team>();

        public string FormFieldJson
        {
            get { return JsonSerializer.Serialize(FormField); }
            set
            {
                if (string.IsNullOrEmpty(value))
                    FormField = new List<FormFieldItem>();
                else
                    FormField = JsonSerializer.Deserialize<List<FormFieldItem>>(value);
            }
        }

        [NotMapped]
        public List<FormFieldItem> FormField { get; set; }
    }
}
