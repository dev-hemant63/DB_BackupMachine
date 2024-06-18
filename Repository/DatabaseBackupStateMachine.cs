using FSM.Enum;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.IO;

namespace FSM.Repository
{
    public class DatabaseBackupStateMachine
    {
        private BackupState currentState;

        public DatabaseBackupStateMachine()
        {
            currentState = BackupState.Idle;
        }

        public void RequestBackup()
        {
            Console.WriteLine("Backup Initiating....");

            string serverName = "103.180.120.159"; //"618994-ABHITRAD\\SQLEXPRESS";
            string databaseName = "db_Name";
            string userName = "something";
            string password = "something";
            string backupPath = @"C:\DB-Backup\";
            string backupFileName = $"{databaseName}_backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            try
            {
                //ServerConnection serverConnection = new ServerConnection(serverName);
                ServerConnection serverConnection = new ServerConnection(serverName, userName, password);
                Server server = new Server(serverConnection);
                Backup backup = new Backup
                {
                    Action = BackupActionType.Database,
                    Database = databaseName
                };
                backup.Devices.AddDevice(backupPath + backupFileName, DeviceType.File);
                backup.Initialize = true;
                backup.SqlBackup(server);
                Console.WriteLine($"Backup completed successfully {DateTime.Now.ToString("dd MM yyyy hh mm ss")} ......");

                DeleteOldBackUp(backupPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Backup failed. Error: " + ex);
            }
        }
        public void DeleteOldBackUp(string rootFolder)
        {
            string[] files = Directory.GetFiles(rootFolder);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                TimeSpan age = DateTime.Now - fileInfo.CreationTime;
                if (age.TotalHours > 24)
                {
                    try
                    {
                        File.Delete(file);
                        Console.WriteLine($"{file} deleted.....");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting {file}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"{file} is not older than 24 hours. Skipping deletion....");
                }
            }
        }
    }
}
