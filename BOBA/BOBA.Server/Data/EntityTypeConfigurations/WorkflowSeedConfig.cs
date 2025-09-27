using BOBA.Server.Data.model;
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
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"2\"}]",
                FormFieldJson = "[{\"Id\":\"personalInfo\",\"Layout\":{\"Type\":\"grid\",\"Columns\":2,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"firstName\",\"Type\":\"text\",\"Label\":\"First Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"lastName\",\"Type\":\"text\",\"Label\":\"Last Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]},{\"Id\":\"contactInfo\",\"Layout\":{\"Type\":\"full-width\"},\"Fields\":[{\"Id\":\"email\",\"Type\":\"email\",\"Label\":\"Email Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"phone\",\"Type\":\"tel\",\"Label\":\"Phone Number\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]},{\"Id\":\"addressInfo\",\"Layout\":{\"Type\":\"combined\"},\"Fields\":[{\"Id\":\"address\",\"Type\":\"text\",\"Label\":\"Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":[{\"Id\":\"cityStateZip\",\"Layout\":{\"Type\":\"grid\",\"Columns\":3,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"city\",\"Type\":\"text\",\"Label\":\"City\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"state\",\"Type\":\"text\",\"Label\":\"State\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"zip\",\"Type\":\"text\",\"Label\":\"Zip Code\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]}]},{\"Id\":\"additionalInfo\",\"Layout\":{\"Type\":\"full-width\"},\"Fields\":[{\"Id\":\"notes\",\"Type\":\"textarea\",\"Label\":\"Additional Notes\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":4,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]}]"
                //NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"2\"}, {\"choiceId\": \"2\", \"nextStateId\": \"3\"}, {\"choiceId\": \"3\", \"nextStateId\": \"4\"}]"
            },
            new TaskFlow
            {
                Id = "2",
                TaskTypeId = "2",  
                CurrentStateId = "2",  // Content Creation
                NextStateJson = "[{\"ChoiceId\":\"1\",\"NextStateId\":\"3\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"1\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\"}]",
                FormFieldJson = "[{\"Id\":\"personalInfo\",\"Layout\":{\"Type\":\"grid\",\"Columns\":2,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"firstName\",\"Type\":\"text\",\"Label\":\"First Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"lastName\",\"Type\":\"text\",\"Label\":\"Last Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]},{\"Id\":\"contactInfo\",\"Layout\":{\"Type\":\"full-width\"},\"Fields\":[{\"Id\":\"email\",\"Type\":\"email\",\"Label\":\"Email Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"phone\",\"Type\":\"tel\",\"Label\":\"Phone Number\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]},{\"Id\":\"addressInfo\",\"Layout\":{\"Type\":\"combined\"},\"Fields\":[{\"Id\":\"address\",\"Type\":\"text\",\"Label\":\"Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":[{\"Id\":\"cityStateZip\",\"Layout\":{\"Type\":\"grid\",\"Columns\":3,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"city\",\"Type\":\"text\",\"Label\":\"City\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"state\",\"Type\":\"text\",\"Label\":\"State\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"zip\",\"Type\":\"text\",\"Label\":\"Zip Code\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]}]},{\"Id\":\"additionalInfo\",\"Layout\":{\"Type\":\"full-width\"},\"Fields\":[{\"Id\":\"notes\",\"Type\":\"textarea\",\"Label\":\"Additional Notes\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":4,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}]}]"
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
