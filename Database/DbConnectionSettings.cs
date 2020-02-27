using System.Runtime.InteropServices;

namespace ConsoleTemplate.Database
{
    internal class DbConnectionSettings
    {
#if DEBUG
        internal string CONNECTION_STRING => $@"Data Source=(LocalDB)\MSSQLLocalDB;Database={Database};MultipleActiveResultSets=True;Integrated Security=true;";
#else
        internal string CONNECTION_STRING => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                     $"Server={Server};Database={Database};User ID={UserName};Password={Password};MultipleActiveResultSets=True;Integrated Security=true;" :
                     $"Server={Server};Database={Database};User ID={UserName};Password={Password};MultipleActiveResultSets=True;";
#endif

        internal string UserName { get; set; } = string.Empty;
        internal string Password { get; set; } = string.Empty;
        internal string Server { get; set; } = string.Empty;
        internal string Database { get; set; } = string.Empty;
        internal string Schema { get; set; } = string.Empty;
        internal string Table { get; set; } = string.Empty;

        internal bool HasUsernameAndPassword => !string.IsNullOrEmpty(UserName) & !string.IsNullOrEmpty(Password);
        internal bool IsUserOrPasswordEmpty => string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password);

        internal bool DatabaseIsUnset => string.IsNullOrWhiteSpace(Database);
        internal bool ServerIsUnset => string.IsNullOrWhiteSpace(Server);
    }
}