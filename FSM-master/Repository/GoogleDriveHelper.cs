using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System.Management;

namespace FSM.Repository
{
    public static class GoogleDriveHelper
    {
        public static string UploadFile(string filePath)
        {
            string credentialsPath = "credentials.json";
            string folderId = "1BiD3RFBIBzWLEblroaAJ3np3qiTfv2Oe";
            GoogleCredential credential;

            try
            {
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.DriveFile);
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "UploadTesting"
                });

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { folderId }
                };

                FilesResource.CreateMediaUpload request;
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    string mimeType = GetMimeType(filePath);
                    request = service.Files.Create(fileMetadata, fileStream, mimeType);
                    request.Fields = "id";

                    // Upload the file and check the response
                    var uploadProgress = request.Upload();
                    if (uploadProgress.Status != UploadStatus.Completed)
                    {
                        Console.WriteLine($"Error uploading file: {uploadProgress.Exception}");
                        return null;
                    }
                }
                var file = request.ResponseBody;

                if (file != null)
                {
                    Console.WriteLine($"Backup Uploaded Successfully and Id is : {file.Id}");
                    return file.Id;
                }
                else
                {
                    Console.WriteLine("Upload failed: request.ResponseBody is null");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
    }
}
