using Microsoft.EntityFrameworkCore;

namespace ConsoleTemplate.Database
{
    internal abstract class BaseContext : DbContext
    {
        protected internal static string? UserName { protected get; set; } = string.Empty;
        protected internal static string Password { protected get; set; } = string.Empty;
        protected internal static string Server { protected get; set; } = string.Empty;
        internal static string DatabaseToUse { get; set; } = string.Empty;
        internal static string TableName { get; set; } = string.Empty;
        internal static string Schema { get; set; } = string.Empty;
    }
}
