using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CleanCode.Mutant.AulaRabbitMQ.Server.Queue
{
    public class QueueManager
    {
        string _hostName = "localhost";
        string _queueName = "queue_mutant";

        public void Receiver()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(_queueName, true, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    channel.BasicConsume(
                                queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

                    Console.WriteLine("Precione qualquer tecla para continuar");
                    Console.ReadLine();
                }
            }
        }
    }
}
