using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TaskTypeSeedConfig : IEntityTypeConfiguration<TaskType>
{
    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        builder.HasData(
            new TaskType 
            { 
                Id = "1", 
                Name = "Social Media Campaign", 
                Description = "Plan, create, and schedule posts for social media platforms." 
            },
            new TaskType 
            { 
                Id = "2", 
                Name = "Ad Campaign", 
                Description = "Create and manage an advertising campaign across different channels." 
            },
            new TaskType 
            { 
                Id = "3", 
                Name = "Email Marketing", 
                Description = "Design and send promotional emails to targeted audiences." 
            },
            new TaskType 
            { 
                Id = "4", 
                Name = "SEO Optimization", 
                Description = "Improve website SEO through keyword research and content updates." 
            },
            new TaskType 
            { 
                Id = "5",
                Name = "Market Research",
                Description = "Analyze competitors, customer behavior, and industry trends." 
            }
        );
    }
}
