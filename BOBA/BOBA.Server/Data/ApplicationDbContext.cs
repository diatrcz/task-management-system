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
    public DbSet<TaskState> TaskStates => Set<TaskState>();
    public DbSet<TaskType> TaskTypes => Set<TaskType>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Taskflow> TaskFlows => Set<Taskflow>();
    public DbSet<Choice> Choices => Set<Choice>();

    public DbSet<TaskField> TaskFields => Set<TaskField>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Task -> Assignee relationship
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);  

        // Task -> Creator relationship
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Task -> CurrentState relationship
        modelBuilder.Entity<Task>()
            .HasOne(t => t.CurrentState)
            .WithMany(ts => ts.CurrentStateTasks)
            .HasForeignKey(t => t.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Workflow -> CurrentState relationship
        modelBuilder.Entity<Taskflow>()
            .HasOne(w => w.CurrentState)
            .WithMany(ts => ts.CurrentStateTaskflows)
            .HasForeignKey(w => w.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Store NextStateJson as nvarchar(max)
        modelBuilder.Entity<Taskflow>()
           .Property(w => w.NextStateJson)
           .HasColumnType("nvarchar(max)");


        modelBuilder.ApplyConfiguration(new TaskStatusSeedConfig());
        modelBuilder.ApplyConfiguration(new TaskTypeSeedConfig());
        modelBuilder.ApplyConfiguration(new TeamSeedConfig());
        modelBuilder.ApplyConfiguration(new WorkflowSeedConfig());
        modelBuilder.ApplyConfiguration(new ChoiceSeedConfig());
    }
}
