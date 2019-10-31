using System.Runtime.InteropServices;

namespace ConsoleTemplate.Database
{
    class CensusTractConnectionInfo
    {
#if DEBUG
        internal static string CONNECTION_STRING => $@"Data Source=(LocalDB)\MSSQLLocalDB;Database={Database};MultipleActiveResultSets=True;Integrated Security=true;";
#else
        internal static string CONNECTION_STRING => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                     $"Server={Server};Database={Database};MultipleActiveResultSets=True;Integrated Security=true;" :
                     $"Server={Server};Database={Database};User ID={UserName};Password={Password};MultipleActiveResultSets=True;";
#endif

        public static string Server { get; set; } = "";
        public static string Database { get; set; } = "";
        public static string Schema { get; set; } = "";
        public static string Table { get; set; } = "";
        public static string Column { get; set; } = "";
        public static string ShapeColumn { get; set; } = "";
        public static string UserName { private get; set; } = "";
        public static string Password { private get; set; } = "";

        public static bool IsUserOrPasswordNullOrEmpty => string.IsNullOrEmpty(UserName) | string.IsNullOrEmpty(Password);

    }
}