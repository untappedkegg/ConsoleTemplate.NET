using ConsoleTemplate.Database;
using ConsoleTemplate.DataParsing;
using ConsoleTemplate.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleTemplate
{
    class Program
    {
        private const string NO_CREDENTIALS = "The Username and/or Password are not set. Please add them to Config.xml";
        private const string NO_SERVER = "The Server is not set. Please add it to Config.xml";
        private const string NO_DATABASE = "The Database is not set. Please add it to Config.xml";

        public static async Task<int> Main(string[] args)
        {
            var showInfo = ConfigUtils.GetConfigBool("showInfo");

            Stopwatch? programTimer = null;

            if (showInfo)
            {
                programTimer = Stopwatch.StartNew();
            }

            Console.WriteLine(Funnies.Funnies.GetLine());
            DownloadUtils.Init();

            try
            {
                DownloadUtils.DeleteDownloads();
            }
            catch (Exception) { }

            // Read Config.xml            
            string zipFile = ConfigUtils.GetConfigValue("zipFile");
            string urlBase = ConfigUtils.GetConfigValue("urlBase");
            string dbServer = ConfigUtils.GetConfigValue("server");
            string dbUser = ConfigUtils.GetConfigValue("user");
            string dbPass = ConfigUtils.GetConfigValue("password");
            string database = ConfigUtils.GetConfigValue("database");
            string schema = ConfigUtils.GetConfigValue("schema");
            string table = ConfigUtils.GetConfigValue("tableName");
            bool deleteDownloads = ConfigUtils.GetConfigBool("deleteDownloadsOnSuccess");

            // Windoze can use Windows Auth, so user and password are not required
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && (string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPass)))
            {
                PrintMessageAndWait(NO_CREDENTIALS);
                return -7;
            }

#if !DEBUG
            if (string.IsNullOrWhiteSpace(dbServer))
            {
                PrintMessageAndWait(NO_SERVER);
                return -8;
            }
#endif
            if (string.IsNullOrWhiteSpace(database))
            {
                PrintMessageAndWait(NO_DATABASE);
                return -9;
            }


            Stopwatch? timer = null;

            var now = DateTime.Now;

            DownloadUtils.CreateDownloadTempDirectory();

            // Init the database
            BaseContext.Server = dbServer;
            BaseContext.UserName = dbUser;
            BaseContext.Password = dbPass;
            BaseContext.Schema = schema;
            BaseContext.TableName = table;
            BaseContext.DatabaseToUse = database;

            // This is only used for testing/LocalDB
            //using (var ctx = new Context())
            //{
            //    ctx.EnsureDatabase();
            //}

            if (showInfo)
            {
                Console.WriteLine($"Connecting to {urlBase.Substring(8, urlBase.IndexOf(".gov") - 4)} - {now}");
                timer = Stopwatch.StartNew();
            }


            // Download the things            
            await DownloadUtils.DownloadFileAsync(urlBase, zipFile);


            if (showInfo)
            {
                timer!.Stop();
                Console.WriteLine($"Downloaded {DownloadUtils.GetDownloadSizeInMb()}MB in {timer.Elapsed.Minutes} minutes {timer.Elapsed.Seconds} seconds");
            }

            DownloadUtils.UnZip(zipFile);


            // Process the data
            Console.WriteLine(Funnies.Funnies.GetLine());

            await DataParser.ParseFileAsync<IKeyed<object>>(DownloadUtils.GetLargestFileInDownload());


            if (showInfo)
            {
                programTimer!.Stop();
                PrintMessageAndWait($"Execution completed in {programTimer.Elapsed.Hours} hours {programTimer.Elapsed.Minutes} minutes {programTimer.Elapsed.Seconds}.{programTimer.Elapsed.Milliseconds} seconds");
            }


            if (deleteDownloads)
            {
                try
                {
                    DownloadUtils.DeleteDownloads();
                }
                catch (Exception e)
                {
                    PrintMessageAndWait(e.Message);
                }
            }

            return 0;
        }

        private static void PrintMessageAndWait(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
