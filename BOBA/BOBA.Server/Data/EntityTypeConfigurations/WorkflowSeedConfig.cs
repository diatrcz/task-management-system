using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class WorkflowSeedConfig : IEntityTypeConfiguration<Taskflow>
{
    public void Configure(EntityTypeBuilder<Taskflow> builder)
    {
        builder.HasData(
            new Taskflow
            {
                Id = "1",
                TaskTypeId = "2", 
                CurrentStateId = "1",  // Initial State
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"2\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"3\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"4\"}]",
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"2\"}, {\"choiceId\": \"2\", \"nextStateId\": \"3\"}, {\"choiceId\": \"3\", \"nextStateId\": \"4\"}]"
            },
            new Taskflow
            {
                Id = "2",
                TaskTypeId = "2",  
                CurrentStateId = "2",  // Content Creation
                NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"3\"}, {\"choiceId\": \"2\", \"nextStateId\": \"4\"}, {\"choiceId\": \"3\", \"nextStateId\": \"5\"}]"
            },
            new Taskflow
            {
                Id = "3",
                TaskTypeId = "2",  
                CurrentStateId = "3",  // Internal Review
                NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"5\"}, {\"choiceId\": \"2\", \"nextStateId\": \"6\"}]"
            },
            new Taskflow
            {
                Id = "4",
                TaskTypeId = "2",  
                CurrentStateId = "4",  // Client Review
                NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"6\"}, {\"choiceId\": \"2\", \"nextStateId\": \"7\"}]"
            },
            new Taskflow
            {
                Id = "5",
                TaskTypeId = "2",  
                CurrentStateId = "5",  // Revisions
                NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"7\"}, {\"choiceId\": \"2\", \"nextStateId\": \"8\"}]"
            },
            new Taskflow
            {
                Id = "6",
                TaskTypeId = "2",  
                CurrentStateId = "6",  // Approved
                NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"8\"}, {\"choiceId\": \"2\", \"nextStateId\": \"9\"}]"
            }
        );
    }
}
