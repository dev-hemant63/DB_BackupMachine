using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FSM.Repository.RcurringServicesAndBilling
{
    public static class JobForRcurringServicesAndBills
    {
        public static async Task<bool> JobForRcurringServices(string connection)
        {
            var isSuccess = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    var res = await con.QueryAsync("SP_JobForRcurringServices",commandType:CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return isSuccess;
        }
        public static async Task<bool> JobForRcurringServicesBilling(string connection)
        {
            var isSuccess = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    var res = await con.QueryAsync("SP_JobForRcurringServicesBilling", commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return isSuccess;
        }
    }
}
