using BOBA.Server.Data.implementation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data.model
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
        public virtual ICollection<TaskFlow> EditRoleWorkflows { get; set; } = new List<TaskFlow>();

        public virtual ICollection<TaskFlow> ReadOnlyRoleWorkflows { get; set; } = new List<TaskFlow>();
    }
}
