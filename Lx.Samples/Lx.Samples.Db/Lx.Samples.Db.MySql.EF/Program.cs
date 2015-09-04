using System;
using System.Data.Entity;
using System.Linq;
using MySql.Data.Entity;

namespace Lx.Samples.Db.MySql.EF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            using (var db = new BloggingContext())
            {
                db.Database.Log = Console.WriteLine;

                db.Database.CreateIfNotExists();

                using (var transaction = db.Database.BeginTransaction())
                {
                    // Create and save a new Blog 
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();

                    var blog = new Blog {Name = name};
                    db.Blogs.Add(blog);
                    db.SaveChanges();

                    // Display all Blogs from the database 
                    var query = from b in db.Blogs
                        orderby b.Name
                        select b;

                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                    transaction.Commit();
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}