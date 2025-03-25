using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOBA.Server.Data
{
    public class User: IdentityUser
    {
        [Required]
        public Role Type { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();

        [InverseProperty("Creator")]
        public virtual ICollection<Task> CreatedTasks { get; set; } = new List<Task>();

        [InverseProperty("Assignee")]
        public virtual ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    }
}
