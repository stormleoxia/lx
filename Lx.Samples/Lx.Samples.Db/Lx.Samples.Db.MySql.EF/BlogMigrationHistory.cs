using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace Lx.Tools.Db.EntityFrameworkPoc
{
    public class BlogMigrationHistory : HistoryContext
    {

        public BlogMigrationHistory(DbConnection connection, string defaultSchema)
            : base(connection, defaultSchema)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
        }
    }
}
