using Microsoft.EntityFrameworkCore;

namespace ConsoleTemplate.Database
{
   internal abstract class BaseContext : DbContext
    {
        protected readonly DbConnectionSettings settings;
        public BaseContext(DbConnectionSettings connectionSettings) : base()// where T : DbConnectionSettings 
        {
            settings = connectionSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Tell it we're using SqlServer and the relevant info
            optionsBuilder.UseSqlServer(settings.CONNECTION_STRING).EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
