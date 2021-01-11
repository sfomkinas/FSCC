using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCC.Core.Data
{
    public class FSCCContext : DbContext
    {
        public FSCCContext() : base(GetConnection(), false)
        {
        }

        public DbSet<JournalLog> JournalLogs { get; set; }
        public DbSet<ProductKind> ProductKinds { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Product> Products { get; set; }

        public static DbConnection GetConnection()
        {
            var connection = ConfigurationManager.ConnectionStrings["FSCCContext"];
            var factory = DbProviderFactories.GetFactory(connection.ProviderName);
            var dbCon = factory.CreateConnection();
            dbCon.ConnectionString = connection.ConnectionString;
            return dbCon;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
