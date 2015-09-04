using System.Data.Entity.Migrations;
using MySql.Data.Entity;

namespace Lx.Tools.Db.EntityFrameworkPoc
{


    internal sealed class Configuration : DbMigrationsConfiguration<CodeFirstMySql.Models.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator()); //it will generate MySql commands instead of SqlServer commands.

            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema)); //here s the thing.



        }

        protected override void Seed(CodeFirstMySql.Models.Context context) { }//ommited
    }
}
