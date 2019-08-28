using ConsoleTemplate.Database;
using ConsoleTemplate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTemplate.DataParsing
{
    internal class DataStreamParser
    {

        private const int BATCH_SIZE = 7_500;
        private const char DELIMITER = '\t';

        private static int SaveData<T>(IEnumerable<T> data) where T : class, IKeyed<object>
        {
            using var context = new Context();
            context.AddRange(data);
            return context.SaveChanges();
        }

        private class Carable : IKeyed<object>
        {
            public object Key { get; set; } = new object();
        }

        internal static Task<TaskStatus> ParseFile(Stream fileStream)
        {
            static Carable createRecord(string seriesId, string year, string month, string value) => new Carable();
            return ParseData(fileStream, data => SaveData(data), createRecord);
        }

        private static Task<TaskStatus> ParseData<T>(Stream fileStream, Func<IEnumerable<T>, int> func, Func<string, string, string, string, T> createRecord) where T : class, IKeyed<object>
        {
            return Task.Run(() =>
            {
                try
                {
                    // We're manually parsing the CSV file(s) for effieciency
                    using (var reader = new StreamReader(fileStream))
                    {
                        // Discard header
                        reader.ReadLine();

                        // Define all variables/pointers outside of loop for memory managment
                        string? line = reader.ReadLine();
                        var dataArray = new List<T>(BATCH_SIZE + 1);
                        T record;
                        string[] tokens;
                        do
                        {
                            tokens = line!.Split(DELIMITER);

                            record = createRecord(tokens[0].TrimEnd(), tokens[1].TrimEnd(), tokens[2].TrimEnd(), tokens[3].Trim());
                            if (record.Key != null)
                            {
                                dataArray.Add(record);
                                // Checkpoint records every 25k, to minimize the SaveChanges penalty
                                if (dataArray.Count > BATCH_SIZE)
                                {
                                    func.Invoke(dataArray.ToArray());
                                    dataArray.Clear();
                                }
                            }

                            line = reader.ReadLine();
                        } while (line != null);
                        // Save any changes which have not been saved already
                        func.Invoke(dataArray);
                        return TaskStatus.RanToCompletion;
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine(e.ToString());
#endif
                    return TaskStatus.Faulted;
                }
            });

        }
    }
}
