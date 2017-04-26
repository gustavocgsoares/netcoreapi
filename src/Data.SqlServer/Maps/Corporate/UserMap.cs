using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Corporate;

namespace Template.Data.SqlServer.Maps.Corporate
{
    public static class UserMap
    {
        public static ModelBuilder MapUser(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<User>();

            // Table & Column Mappings
            entity.ToTable("User", "dbo");

            // Primary Key
            entity.HasKey(p => new { p.Id });
            entity.Property(p => p.Id)
               .ForSqlServerHasColumnType("uniqueidentifier")
               .ForSqlServerHasDefaultValueSql("newid()")
               .IsRequired();

            // Properties
            entity.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Password)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(p => p.ProfileImage)
                .HasMaxLength(100)
                .IsRequired();

            //// Relationships
            ////...

            return modelBuilder;
        }
    }
}