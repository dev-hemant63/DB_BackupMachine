using FSM.Models;
using FSM.Repository;
using FSM.Repository.RcurringServicesAndBilling;
using System;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // DB BackUP
        //DatabaseBackupStateMachine fsm = new DatabaseBackupStateMachine();
        //fsm.RequestBackup();

        // Other Tasks
        string filePath = "appsetting.json";
        string jsonString = File.ReadAllText(filePath);

        AppConfig config = JsonSerializer.Deserialize<AppConfig>(jsonString);
        ConnectionConfig connectionConfig = new ConnectionConfig();
        connectionConfig.SqlConnection = config.Mode == "Test" ? config.ConnectionStrings.Local_DB : config.ConnectionStrings.Live_DB;


        var isSuccess = await JobForRcurringServicesAndBills.JobForRcurringServices(connectionConfig.SqlConnection);
        if (!isSuccess)
        {
            Console.WriteLine("The job rcurring services could not be run, there is a problem.");
        }
        else
        {
            Console.WriteLine("The job rcurring services complete successfully.");
        }
        if (isSuccess)
        {
            isSuccess = await JobForRcurringServicesAndBills.JobForRcurringServicesBilling(connectionConfig.SqlConnection);
            if (!isSuccess)
            {
                Console.WriteLine("The job rcurring services billing could not be run, there is a problem.");
            }
            else
            {
                Console.WriteLine("The job rcurring services billing complete successfully.");
            }
        }

        Console.ReadLine();
    }
}
