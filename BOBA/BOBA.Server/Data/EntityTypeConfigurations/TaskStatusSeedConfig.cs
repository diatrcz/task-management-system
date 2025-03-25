using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TaskStatusSeedConfig : IEntityTypeConfiguration<TaskStatus>
{
    public void Configure(EntityTypeBuilder<TaskStatus> builder)
    {
        builder.HasData(
            new TaskStatus 
            { 
                Id = "1", 
                Name = "Planning", 
                Description = "Define campaign goals and target audience.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "2", 
                Name = "Content Creation", 
                Description = "Write copy and design assets.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "3", 
                Name = "Internal Review", 
                Description = "Content is reviewed internally by the marketing team.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "4", 
                Name = "Client Review", 
                Description = "Waiting for client approval.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "5", 
                Name = "Revisions", 
                Description = "Client requested changes, content needs updating.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "6", 
                Name = "Approved", 
                Description = "Content is finalized and ready for scheduling.",
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "7", 
                Name = "Scheduled", 
                Description = "Campaign is scheduled for publishing.", 
                IsFinal = false 
            },
            new TaskStatus 
            { 
                Id = "8", 
                Name = "Published", 
                Description = "Content has been posted on selected platforms.", 
                IsFinal = true 
            },
            new TaskStatus 
            { 
                Id = "9", 
                Name = "Cancelled", 
                Description = "The task is completed and archived.", 
                IsFinal = true 
            }
        );
    }

}
