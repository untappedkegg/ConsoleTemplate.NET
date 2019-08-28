using ConsoleTemplate.Models;
using Microsoft.EntityFrameworkCore;
#if !DEBUG
using System.Runtime.InteropServices;
#endif

namespace ConsoleTemplate.Database
{
    internal class Context : BaseContext
    {
#if DEBUG
        internal static string CONNECTION_STRING => $@"Data Source=(LocalDB)\MSSQLLocalDB;Database={DatabaseToUse};MultipleActiveResultSets=True;Integrated Security=true;";
#else
        internal static string CONNECTION_STRING => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                     $"Server={Server};Database={DatabaseToUse};User ID={UserName};Password={Password};MultipleActiveResultSets=True;Integrated Security=true;" :
                     $"Server={Server};Database={DatabaseToUse};User ID={UserName};Password={Password};MultipleActiveResultSets=True;";
#endif
        internal void EnsureDatabase()
        {
            // This is equivalent to the Framework
            // DropCreateDatabaseAlways Migration Strategy
            if (!hasWipedDb)
            {
                hasWipedDb = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        private static volatile bool hasWipedDb = false;


#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        public virtual DbSet<IKeyed<object>> KeyedEntity { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Tell it we're using SqlServer and the relevant info
            optionsBuilder.UseSqlServer(CONNECTION_STRING).EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set the schema and table name via the fluent api
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Entity<IKeyed<object>>().ToTable(TableName);

            base.OnModelCreating(modelBuilder);
        }
    }
}
