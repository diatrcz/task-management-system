using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class WorkflowSeedConfig : IEntityTypeConfiguration<TaskFlow>
{
    public void Configure(EntityTypeBuilder<TaskFlow> builder)
    {
        builder.HasData(
            new TaskFlow
            {
                Id = "1",
                TaskTypeId = "2", 
                CurrentStateId = "1",  // Initial State
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"2\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"2\"}, {\"choiceId\": \"2\", \"nextStateId\": \"3\"}, {\"choiceId\": \"3\", \"nextStateId\": \"4\"}]"
            },
            new TaskFlow
            {
                Id = "2",
                TaskTypeId = "2",  
                CurrentStateId = "2",  // Content Creation
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"3\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"1\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"3\"}, {\"choiceId\": \"2\", \"nextStateId\": \"4\"}, {\"choiceId\": \"3\", \"nextStateId\": \"5\"}]"
            },
            new TaskFlow
            {
                Id = "3",
                TaskTypeId = "2",  
                CurrentStateId = "3",  // Internal Review
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"4\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"2\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"4\"}, {\"choiceId\": \"2\", \"nextStateId\": \"6\"}]"
            },
            new TaskFlow
            {
                Id = "4",
                TaskTypeId = "2",  
                CurrentStateId = "4",  // Client Review
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"5\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"3\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"5\"}, {\"choiceId\": \"2\", \"nextStateId\": \"7\"}]"
            },
            new TaskFlow
            {
                Id = "5",
                TaskTypeId = "2",  
                CurrentStateId = "5",  // Revisions
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"6\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"4\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"7\"}, {\"choiceId\": \"2\", \"nextStateId\": \"8\"}]"
            },
            new TaskFlow
            {
                Id = "6",
                TaskTypeId = "2",  
                CurrentStateId = "6",  // Approved
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"8\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"5\"}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"8\"}, {\"choiceId\": \"2\", \"nextStateId\": \"9\"}]"
            }
        );
    }
}
