using BOBA.Server.Data.EntityTypeConfigurations;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = BOBA.Server.Data.implementation.Task;

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
    public DbSet<TaskFlow> TaskFlows => Set<TaskFlow>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<TaskField> TaskFields => Set<TaskField>();
    public DbSet<FormDocument> FormDocuments => Set<FormDocument>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<TaskDocType> TaskDocTypes => Set<TaskDocType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.CurrentState)
            .WithMany(ts => ts.CurrentStateTasks)
            .HasForeignKey(t => t.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<TaskFlow>()
            .HasOne(w => w.CurrentState)
            .WithMany(ts => ts.CurrentStateTaskflows)
            .HasForeignKey(w => w.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskFlow>()
            .Property(w => w.NextStateJson)
            .HasColumnType("nvarchar(max)");

        modelBuilder.ApplyConfiguration(new TaskStatusSeedConfig());
        modelBuilder.ApplyConfiguration(new TaskTypeSeedConfig());
        modelBuilder.ApplyConfiguration(new TeamSeedConfig());
        modelBuilder.ApplyConfiguration(new WorkflowSeedConfig());
        modelBuilder.ApplyConfiguration(new ChoiceSeedConfig());
        modelBuilder.ApplyConfiguration(new TaskDocTypeSeedConfig());
        modelBuilder.ApplyConfiguration(new TaskFieldSeedConfig());

        modelBuilder.Entity("TaskDocTypeTaskType").HasData(
            new { TaskDocTypesId = "1", TaskTypesId = "1" },
            new { TaskDocTypesId = "1", TaskTypesId = "2" },
            new { TaskDocTypesId = "2", TaskTypesId = "2" },
            new { TaskDocTypesId = "3", TaskTypesId = "2" },
            new { TaskDocTypesId = "4", TaskTypesId = "2" },
            new { TaskDocTypesId = "1", TaskTypesId = "3" },
            new { TaskDocTypesId = "2", TaskTypesId = "3" },
            new { TaskDocTypesId = "4", TaskTypesId = "4" }
        );
    }
}
