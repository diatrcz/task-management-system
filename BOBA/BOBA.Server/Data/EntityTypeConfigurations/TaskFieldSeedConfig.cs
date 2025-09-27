using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BOBA.Server.Data.model;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TaskFieldSeedConfig : IEntityTypeConfiguration<TaskField>
{
    public void Configure(EntityTypeBuilder<TaskField> builder)
    {
        builder.HasData(
            new TaskField
            {
                Id = "1",
                Name = "Due Date",
                Description = "Deadline for task completion.",
                Validation = @"^\d{4}-\d{2}-\d{2}$",
                Options = null
            },
            new TaskField
            {
                Id = "2",
                Name = "Priority",
                Description = "Task urgency level.",
                Validation = @"^(Low|Medium|High|Urgent)$",
                Options = "Low,Medium,High,Urgent"
            },
            new TaskField
            {
                Id = "3",
                Name = "Assigned To",
                Description = "Person responsible for the task.",
                Validation = null,
                Options = null
            },
            new TaskField
            {
                Id = "4",
                Name = "Channel",
                Description = "Marketing channel the task belongs to.",
                Validation = @"^(Email|Social Media|SEO|Paid Ads|Events)$",
                Options = "Email,Social Media,SEO,Paid Ads,Events"
            },
            new TaskField
            {
                Id = "5",
                Name = "Budget",
                Description = "Estimated task budget.",
                Validation = @"^\d+(\.\d{1,2})?$",
                Options = null
            }
        );
    }
}
