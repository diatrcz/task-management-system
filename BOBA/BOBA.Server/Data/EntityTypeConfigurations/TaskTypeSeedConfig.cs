using BOBA.Server.Data.model;
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
                StarterTeamId = "1"
            },
            new TaskType 
            { 
                Id = "2", 
                Name = "Ad Campaign",
                StarterTeamId = "1"
            },
            new TaskType 
            { 
                Id = "3", 
                Name = "Email Marketing",
                StarterTeamId = "2"
            },
            new TaskType 
            { 
                Id = "4", 
                Name = "SEO Optimization",
                StarterTeamId = "3"
            },
            new TaskType 
            { 
                Id = "5",
                Name = "Market Research",
                StarterTeamId = "4"
            }
        );
    }
}
