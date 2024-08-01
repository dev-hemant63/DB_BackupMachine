using System.Net.Mail;
using System.Net;

namespace FSM.Repository
{
    public static class BackUPEmailSender
    {
        public static async Task<AppResponse> Send(string message)
        {
            var response = new AppResponse
            {
                responseCode = 500,
                responseMessage = "An error has ocuured try after sometime!"
            };
            try
            {
                MailMessage massage = new MailMessage();
                massage.From = new MailAddress("DoNotReply@finnid.in");
                massage.Subject = message;
                massage.To.Add(new MailAddress("parijatam.raam@gmail.com"));
                massage.Body = message;
                massage.IsBodyHtml = false;

                //Attachment attachment = new Attachment(AttachmentURL);
                //massage.Attachments.Add(attachment);


                var smtpclient = new SmtpClient("mail5005.site4now.net")
                {
                    Port = 8889,
                    Credentials = new NetworkCredential("DoNotReply@finnid.in", "123@Finnid#"),
                    EnableSsl = false
                };
                smtpclient.Send(massage);
                response.responseMessage = "Email send successfully!";
                response.responseCode = 200;
            }
            catch (Exception ex)
            {
                response.responseMessage = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return response;
        }
    }
    public class AppResponse
    {
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
    }
}
