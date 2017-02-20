using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class StatsDbContext : DbContext
    {
        public StatsDbContext()
        {
            // Not code first, so disable migrations
            Database.SetInitializer<StatsDbContext>(null);
        }

        public DbSet<Model.servers> Servers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Set database not to pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
