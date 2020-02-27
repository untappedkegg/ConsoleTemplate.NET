using ConsoleTemplate.Models;
using Microsoft.EntityFrameworkCore;
#if !DEBUG
using System.Runtime.InteropServices;
#endif

namespace ConsoleTemplate.Database
{
    internal class Context : BaseContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public Context(DbConnectionSettings connectionSettings) : base(connectionSettings) { }

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IKeyed<object>>().ToTable(settings.Table);

            modelBuilder.HasDefaultSchema(settings.Schema);

            base.OnModelCreating(modelBuilder);
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
