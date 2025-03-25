using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TaskStatusSeedConfig : IEntityTypeConfiguration<TaskState>
{
    public void Configure(EntityTypeBuilder<TaskState> builder)
    {
        builder.HasData(
            new TaskState 
            { 
                Id = "1", 
                Name = "Planning", 
                Description = "Define campaign goals and target audience.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "2", 
                Name = "Content Creation", 
                Description = "Write copy and design assets.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "3", 
                Name = "Internal Review", 
                Description = "Content is reviewed internally by the marketing team.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "4", 
                Name = "Client Review", 
                Description = "Waiting for client approval.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "5", 
                Name = "Revisions", 
                Description = "Client requested changes, content needs updating.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "6", 
                Name = "Approved", 
                Description = "Content is finalized and ready for scheduling.",
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "7", 
                Name = "Scheduled", 
                Description = "Campaign is scheduled for publishing.", 
                IsFinal = false 
            },
            new TaskState 
            { 
                Id = "8", 
                Name = "Published", 
                Description = "Content has been posted on selected platforms.", 
                IsFinal = true 
            },
            new TaskState 
            { 
                Id = "9", 
                Name = "Cancelled", 
                Description = "The task is completed and archived.", 
                IsFinal = true 
            }
        );
    }

}
