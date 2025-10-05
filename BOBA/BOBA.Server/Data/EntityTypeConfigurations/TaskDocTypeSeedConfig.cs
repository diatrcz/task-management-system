using BOBA.Server.Data.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TaskDocTypeSeedConfig : IEntityTypeConfiguration<TaskDocType>
{
    public void Configure(EntityTypeBuilder<TaskDocType> builder)
    {
        builder.HasData(
             new TaskDocType
             {
                 Id = "1",
                 Name = "Campaign Brief",
                 Type = "pdf",
                 Description = "A PDF document outlining the campaign overview and goals."
             },
            new TaskDocType
            {
                Id = "2",
                Name = "Client Contract",
                Type = "docx",
                Description = "A Word document containing the legal agreement with the client."
            },
            new TaskDocType
            {
                Id = "3",
                Name = "Design Mockup",
                Type = "pptx",
                Description = "A PowerPoint presentation of design drafts and visual ideas."
            },
            new TaskDocType
            {
                Id = "4",
                Name = "Performance Report",
                Type = "xlsx",
                Description = "An Excel spreadsheet with performance analytics and KPIs."
            }
        );
    }
}
