using ConsoleTemplate.Database;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleTemplate.DataParsing
{
    internal static class DataCalculations
    {
        internal const int COMMAND_TIMEOUT = 600;

        internal static async Task<int> SetCTIDFP()
        {
            // We subtract 10 from the cutoff year here so that we can calculate the 10 year deltas
            using SqlConnection conn = new SqlConnection(Context.CONNECTION_STRING);
            try
            {
                conn.Open();

                using SqlCommand sqlCommand = new SqlCommand(
                    //$@"UPDATE
	                   //        {BaseContext.DatabaseToUse}.{BaseContext.Schema}.{BaseContext.TableName} 
		                  // SET CTIDFP = CT.GEOID2, Has_New_Location = 0
		                  // FROM
		                  //     {BaseContext.DatabaseToUse}.{BaseContext.Schema}.{BaseContext.TableName} A
		                  // JOIN
		                  //     BoundaryGEO.BOUNDARYGEO.TIGER15_GEO_TRACT ct
		                  // ON
		                  //     geography::STGeomFromText('POINT('+convert(varchar(20),A.X)+' '+convert(varchar(20),A.Y)+')',4326).STIntersects(ct.shape)=1
                    //       WHERE
                    //            Has_New_Location = 1"
                    ""
                    , conn)
                {
                    CommandTimeout = COMMAND_TIMEOUT
                };
                return await sqlCommand.ExecuteNonQueryAsync();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
