using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities.Support;

namespace Template.Data.SqlServer.Maps.Support
{
    public static class FailureReportMap
    {
        ////public static ModelBuilder MapFailureReport(this ModelBuilder modelBuilder)
        ////{
        ////    var entity = modelBuilder.Entity<FailureReport>();

        ////    // Table & Column Mappings
        ////    entity.ToTable("FailureReport", "dbo");

        ////    // Primary Key
        ////    entity.HasKey(p => new { p.Id });
        ////    entity.Property(p => p.Id).UseSqlServerIdentityColumn();

        ////    // Properties
        ////    entity.Property(p => p.Title)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    entity.Property(p => p.Message)
        ////        .IsRequired();

        ////    entity.Property(p => p.Report)
        ////        .HasMaxLength(5000);

        ////    entity.Property(p => p.UserId)
        ////        .HasMaxLength(30)
        ////        .IsRequired();

        ////    entity.Property(p => p.IPAddress)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    entity.Property(p => p.Platform.Cordova)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    entity.Property(p => p.Platform.Device)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    entity.Property(p => p.Platform.Model)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    entity.Property(p => p.Platform.Version)
        ////        .HasMaxLength(100)
        ////        .IsRequired();

        ////    // Relationships
        ////    //...

        ////    return modelBuilder;
        ////}
    }
}