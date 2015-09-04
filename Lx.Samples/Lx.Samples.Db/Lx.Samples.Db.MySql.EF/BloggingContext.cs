using System.Data.Entity;

namespace Lx.Samples.Db.MySql.EF
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
            : base("MySQLConnection")
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().MapToStoredProcedures();
            modelBuilder.Entity<Blog>().MapToStoredProcedures();
        }
    }
}