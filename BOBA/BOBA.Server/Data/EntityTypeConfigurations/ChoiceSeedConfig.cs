using BOBA.Server.Data.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class ChoiceSeedConfig : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> builder)
    {
        builder.HasData(
            new Choice
            {
                Id = "1",
                Name = "Approve, Proceed to Next Phase",
                Description = "Approve the task and move it forward to the next phase of the workflow."
            },
            new Choice
            {
                Id = "2",
                Name = "Needs Further Review",
                Description = "The task requires additional review before proceeding further."
            },
            new Choice
            {
                Id = "3",
                Name = "Reject, Task Requires Redoing",
                Description = "The task does not meet the requirements and needs to be completely reworked."
            },
            new Choice
            {
                Id = "4",
                Name = "Approve with Minor Adjustments",
                Description = "Approve the task but with some minor revisions or improvements."
            }
        );
    }
}
