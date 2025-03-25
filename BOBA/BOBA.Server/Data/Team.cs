using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data
{
    public class Team
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        [InverseProperty("EditRole")]
        public virtual ICollection<Taskflow> EditRoleWorkflows { get; set; } = new List<Taskflow>();

        public virtual ICollection<Taskflow> ReadOnlyRoleWorkflows { get; set; } = new List<Taskflow>();
    }
}
