using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Data.SqlServer.Maps.Corporate;
using Template.Data.SqlServer.Maps.Support;
using Template.Domain.Entities.Corporate;
using Template.Domain.Entities.Support;

namespace Template.Data.SqlServer.Models
{
    public partial class TemplateDbContext : DbContext
    {
        #region Fields | Members
        [ThreadStatic]
        private static TemplateDbContext current;

        private static string connectionString;
        #endregion

        #region Constructors | Destructors
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options, IOptions<CrossCutting.Configurations.Data> data)
            : base(options)
        {
            connectionString = data.Value.SqlServer.ConnectionString;
        }
        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<Suggestion> Suggestions { get; set; }
        #endregion

        #region Static methods
        public static TemplateDbContext Current(DbContextOptions<TemplateDbContext> options, IOptions<CrossCutting.Configurations.Data> data)
        {
            if (current == null)
            {
                current = new TemplateDbContext(options, data);
            }

            return current;
        }
        #endregion

        #region DbContext Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapSuggestion();
            ////modelBuilder.MapFailureReport();
            modelBuilder.MapUser();

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}