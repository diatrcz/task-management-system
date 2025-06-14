﻿using BOBA.Server.Data.EntityTypeConfigurations;
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
        modelBuilder.Entity<TaskFlow>()
            .HasOne(w => w.CurrentState)
            .WithMany(ts => ts.CurrentStateTaskflows)
            .HasForeignKey(w => w.CurrentStateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Store NextStateJson as nvarchar(max)
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
    }
}
