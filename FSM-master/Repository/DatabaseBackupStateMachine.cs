using FSM.Enum;
using Microsoft.Data.SqlClient;
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

            string serverName = "618994-ABHITRAD\\SQLEXPRESS";
            string databaseName = "finnid_live2";
            string userName = "sa";
            string password = "786@Raam";

            //string serverName = "DESKTOP-53IIA9U";
            //string databaseName = "Hangfire_DB";


            //string backupDirectory = @"E:\DB-Backup\";
            string backupDirectory = @"C:\DB-Backup\";
            string backupFileName = $"{databaseName}_backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            try
            {
                //ServerConnection serverConnection = new ServerConnection(serverName);
                ServerConnection serverConnection = new ServerConnection(serverName, userName, password);
                try
                {
                    serverConnection.Connect();
                    Console.WriteLine("Connection to the server established successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to the server: {ex.Message}");
                    return;
                }

                Server server = new Server(serverConnection);
                Backup backup = new Backup
                {
                    Action = BackupActionType.Database,
                    Database = databaseName
                };
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }
                var filePath = backupDirectory + backupFileName;
                backup.Devices.AddDevice(filePath, DeviceType.File);
                backup.Initialize = true;
                backup.SqlBackup(server);

                GoogleDriveHelper.UploadFile(filePath);

                BackUPEmailSender.Send($"Backup completed successfully {DateTime.Now.ToString("dd MM yyyy hh mm ss")}. Go to Google Drive And Check It.");
                Console.WriteLine($"Backup completed successfully {DateTime.Now.ToString("dd MM yyyy hh mm ss")} and to director email......");

                DeleteOldBackUp(backupDirectory);
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
                if (age.TotalHours > 48)
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
