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
                Name = "firstName",
                Type =  "text",
                Label = "First Name",
                Validation = "^[A-Za-z]+$",
            },
            new TaskField
            {
                Id = "2",
                Name = "lastName",
                Type =  "text",
                Label = "Last Name",
                Validation = "^[A-Za-z]+$",
            },
            new TaskField
            {
                Id = "3",
                Name = "email",
                Type =  "email",
                Label = "E-mail",
                Placeholder = "email@email.com",
                Validation = "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$",
            },
            new TaskField
            {
                Id = "4",
                Name = "phone",
                Type =  "text",
                Label = "Phone Number",
                Placeholder = "+36201234567",
                Validation = "^\\+?[0-9\\s\\-()]{7,15}$",
            },
            new TaskField
            {
                Id = "5",
                Name = "address",
                Type =  "text",
                Label = "Address",
            },
            new TaskField
            {
                Id = "6",
                Name = "city",
                Type = "text" ,
                Label = "City",
                Validation = "^[A-Za-z]+$",
            },
            new TaskField
            {
                Id = "7",
                Name = "state",
                Type =  "text",
                Label = "State",
                Validation = "^[A-Za-z]+$",
            },
            new TaskField
            {
                Id = "8",
                Name = "zip",
                Type =  "text",
                Label = "Zip",
                Validation = "^[0-9]+$",
            },
            new TaskField
            {
                Id = "9",
                Name = "notes",
                Type = "textarea",
                Label = "Additional Notes",
            }
        );
    }
}
