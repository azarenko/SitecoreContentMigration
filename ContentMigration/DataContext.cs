using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentMigration
{
    public class SourceDbContext : DbContext
    {
        public SourceDbContext(DbContextOptions<SourceDbContext> options) : base(options) { }
        public DbSet<AccessControl> AccessControl { get; set; }
        public DbSet<Archive> Archive { get; set; }
        public DbSet<ArchivedFields> ArchivedFields { get; set; }
        public DbSet<ArchivedItems> ArchivedItems { get; set; }
        public DbSet<ArchivedVersions> ArchivedVersions { get; set; }
        public DbSet<Blobs> Blobs { get; set; }
        public DbSet<ClientData> ClientData { get; set; }
        public DbSet<Descendants> Descendants { get; set; }
        public DbSet<EventQueue> EventQueue { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<IDTable> IDTable { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<PublishQueue> PublishQueue { get; set; }
        public DbSet<Shadows> Shadows { get; set; }
        public DbSet<SharedFields> SharedFields { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<UnversionedFields> UnversionedFields { get; set; }
        public DbSet<VersionedFields> VersionedFields { get; set; }
        public DbSet<WorkflowHistory> WorkflowHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Descendants>()
                .HasKey(d => new { d.Ancestor, d.Descendant });  // Composite key configuration
        }
    }

    public class TargetDbContext : DbContext
    {
        public TargetDbContext(DbContextOptions<TargetDbContext> options) : base(options) { }
        public DbSet<AccessControl> AccessControl { get; set; }
        public DbSet<Archive> Archive { get; set; }
        public DbSet<ArchivedFields> ArchivedFields { get; set; }
        public DbSet<ArchivedItems> ArchivedItems { get; set; }
        public DbSet<ArchivedVersions> ArchivedVersions { get; set; }
        public DbSet<Blobs> Blobs { get; set; }
        public DbSet<ClientData> ClientData { get; set; }
        public DbSet<Descendants> Descendants { get; set; }
        public DbSet<EventQueue> EventQueue { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<IDTable> IDTable { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<PublishQueue> PublishQueue { get; set; }
        public DbSet<Shadows> Shadows { get; set; }
        public DbSet<SharedFields> SharedFields { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<UnversionedFields> UnversionedFields { get; set; }
        public DbSet<VersionedFields> VersionedFields { get; set; }
        public DbSet<WorkflowHistory> WorkflowHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Descendants>()
                .HasKey(d => new { d.Ancestor, d.Descendant });  // Composite key configuration
        }
    }
}
