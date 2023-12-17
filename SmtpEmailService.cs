using System.Net.Mail;
using System.Net;


namespace MusicLibraryApp
{
    public class SmtpEmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
            
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("emailata8@gmail.com", "hcom fcbt cijz ecxt");
                client.Port = 587;
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("emailata8@gmail.com"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
  

