using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryApp
{
    public class EmailQueueConsumer
    {
        private readonly IEmailService _emailService;

        public EmailQueueConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Consume()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("email_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // mesajı parçalayıp email, subject ve message olarak ayırıyoruz
                    var parts = message.Split(';');
                    if (parts.Length == 3)
                    {
                        var email = parts[0];
                        var subject = parts[1];
                        var emailMessage = parts[2];

                        // Email servisimizi kullanarak email gönderiyoruz
                        _emailService.SendEmailAsync(email, subject, emailMessage).Wait();
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: "email_queue", autoAck: false, consumer: consumer);

           
            }
        }
    }
}
