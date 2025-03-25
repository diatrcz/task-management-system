using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class WorkflowSeedConfig : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder.HasData(
            // Planning → Content Creation (Approved)
            new Workflow
            {
                Id = "1",
                TaskTypeId = "2",
                CurrentStateId = "1", 
                NextStateId = "2", 
                Approved = true
            },

            // Content Creation → Planning (Declined)
            new Workflow
            {
                Id = "2",
                TaskTypeId = "2",
                CurrentStateId = "2",
                NextStateId = "1",
                Approved = false
            },

            // Content Creation → Internal Review (Approved)
            new Workflow
            {
                Id = "3",
                TaskTypeId = "2",
                CurrentStateId = "2", 
                NextStateId = "3", 
                Approved = true
            },

            // Internal Review → Content Creation (Declined)
            new Workflow
            {
                Id = "4",
                TaskTypeId = "2",
                CurrentStateId = "3",
                NextStateId = "2",
                Approved = false
            },

            // Internal Review → Client Review (Approved)
            new Workflow
            {
                Id = "5",
                TaskTypeId = "2",
                CurrentStateId = "3", 
                NextStateId = "4", 
                Approved = true
            },

            // Client Review → Internal Review (Declined)
            new Workflow
            {
                Id = "6",
                TaskTypeId = "2",
                CurrentStateId = "4",
                NextStateId = "3",
                Approved = false
            },


            // Client Review → Approved (Approved)
            new Workflow
            {
                Id = "7",
                TaskTypeId = "2",
                CurrentStateId = "4", 
                NextStateId = "6", 
                Approved = true
            },

            // Approved → Client Review (Declined)
            new Workflow
            {
                Id = "8",
                TaskTypeId = "2",
                CurrentStateId = "6",
                NextStateId = "4",
                Approved = false
            },

            // Approved → Published (Approved)
            new Workflow
            {
                Id = "9",
                TaskTypeId = "2",
                CurrentStateId = "6", 
                NextStateId = "8", 
                Approved = true
            },

            // Cancellation path: Any active state → Cancelled
            new Workflow
            {
                Id = "10",
                TaskTypeId = "2",
                CurrentStateId = "1", //Planning
                NextStateId = "9", 
                Approved = true
            },

            new Workflow
            {
                Id = "11",
                TaskTypeId = "2",
                CurrentStateId = "2", //Content Creation
                NextStateId = "9",
                Approved = true
            },

            new Workflow
            {
                Id = "12",
                TaskTypeId = "2",
                CurrentStateId = "3", //Internal review
                NextStateId = "9",
                Approved = true
            },

            new Workflow
            {
                Id = "13",
                TaskTypeId = "2",
                CurrentStateId = "4", //Client review
                NextStateId = "9",
                Approved = true
            },

            new Workflow
            {
                Id = "14",
                TaskTypeId = "2",
                CurrentStateId = "6", //Approved
                NextStateId = "9",
                Approved = true
            }
        );
    }
}
