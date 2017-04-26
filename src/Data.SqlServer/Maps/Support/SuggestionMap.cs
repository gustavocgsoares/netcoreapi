using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Support;

namespace Template.Data.SqlServer.Maps.Support
{
    public static class SuggestionMap
    {
        public static ModelBuilder MapSuggestion(this ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Suggestion>();

            // Table & Column Mappings
            entity.ToTable("Suggestion", "dbo");

            // Primary Key
            entity.HasKey(p => new { p.Id });
            entity.Property(p => p.Id).UseSqlServerIdentityColumn();

            // Properties
            entity.Property(p => p.Title)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Message)
                .HasMaxLength(5000)
                .IsRequired();

            entity.Property(p => p.User)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(p => p.IPAddress)
                .HasMaxLength(100)
                .IsRequired();

            //// Relationships
            ////...

            return modelBuilder;
        }
    }
}