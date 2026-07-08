namespace TaskManagement.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Password)
            .IsRequired();

        // to use _userRoles instead of the navigation property
        var navigation = builder.Metadata.FindNavigation(nameof(User.UserRoles));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
