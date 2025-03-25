
using BOBA.Server.Data.EntityTypeConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<TaskStatus> TaskStatuses => Set<TaskStatus>();
    public DbSet<TaskType> TaskTypes => Set<TaskType>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Workflow> Workflows => Set<Workflow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Task -> Assignee relationship: Prevent cascade delete
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        // Task -> Creator relationship: Prevent cascade delete
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        // Task -> Workflow relationship: Prevent cascade delete
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Workflow)
            .WithMany()
            .HasForeignKey(t => t.WorkflowId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        // Workflow -> CurrentState relationship
        modelBuilder.Entity<Workflow>()
            .HasOne(w => w.CurrentState)
            .WithMany(ts => ts.CurrentStateWorkflows)
            .HasForeignKey(w => w.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        // Workflow -> NextState relationship
        modelBuilder.Entity<Workflow>()
            .HasOne(w => w.NextState)
            .WithMany(ts => ts.NextStateWorkflows)
            .HasForeignKey(w => w.NextStateId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete


        modelBuilder.ApplyConfiguration(new TaskStatusSeedConfig());
        modelBuilder.ApplyConfiguration(new TaskTypeSeedConfig());
        modelBuilder.ApplyConfiguration(new TeamSeedConfig());
        modelBuilder.ApplyConfiguration(new WorkflowSeedConfig());
    }
}
