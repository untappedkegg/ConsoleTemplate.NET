using ConsoleTemplate.Database;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTemplate.DataParsing
{
    internal static class DataParser
    {
        private const int BATCH_SIZE = 7_500;
        public const string GEOCODE_ADDRESS = "https://services.engagementnetwork.org/api-uploader/geocode";

        internal static async Task<TaskStatus> ParseFileAsync<T>(string fileName, Action<double> updateProgress, DbConnectionSettings settings)
        {
            return await ParseDataAsync(fileName, updateProgress, settings);
        }

        private static async Task<TaskStatus> ParseDataAsync(string fileName, Action<double> updateProgress, DbConnectionSettings settings)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    using (var stream = new StreamReader(fileName))
                    {

                        var ctx = new Context(settings);

                        await ctx.SaveChangesAsync();
                        updateProgress(4);
                        ctx.Dispose();
                    }
                    return TaskStatus.RanToCompletion;


                } else
                {
                    Console.WriteLine($"File Not Found: {fileName}");
                    return TaskStatus.Canceled;
                }

            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
                return TaskStatus.Faulted;
            }


        }
    }
}
