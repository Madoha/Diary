using Diary.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diary.DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);

        builder.HasData(new List<Role>()
        {
            new Role()
            {
                Id = 1,
                Name = "User"
            },
            new Role()
            {
                Id = 2,
                Name = "Admin"
            },
            new Role()
            {
                Id = 3,
                Name = "Moderator"
            },
        });
    }
}