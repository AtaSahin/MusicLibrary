using System.Net.Mail;
using System.Net;
using RabbitMQ.Client;
using System.Text;


namespace MusicLibraryApp
{
    public class RabbitMQService : IEmailService
    {
        public async Task SendEmailAsyncWithQueue(string email, string subject, string message)
        {
            // Send email using SMTP
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

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            // Send email data to RabbitMQ queue
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("email_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var emailData = Encoding.UTF8.GetBytes($"{email};{subject};{message}");

                channel.BasicPublish(exchange: "", routingKey: "email_queue", basicProperties: null, body: emailData);
            }
        }// Send email using ONLY SMTP
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
  

