using BOBA.Server.Data.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOBA.Server.Data.EntityTypeConfigurations;

public class TeamSeedConfig : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasData(
            new Team
            {
                Id = "1",
                Name = "Content Team"
            },
            new Team
            {
                Id = "2",
                Name = "Design Team"
            },
            new Team
            {
                Id = "3",
                Name = "Advertising Team"
            },
            new Team
            {
                Id = "4",
                Name = "SEO & Analytics Team"
            },
            new Team
            {
                Id = "5",
                Name = "Client Management"
            }
        );
    }
}
