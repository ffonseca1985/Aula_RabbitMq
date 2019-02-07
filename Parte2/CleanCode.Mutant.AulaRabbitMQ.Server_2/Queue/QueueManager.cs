using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace CleanCode.Mutant.AulaRabbitMQ.Server_2.Queue
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

                        Thread.Sleep(5000);

                        //Reconhece apenas 1 por vez!!
                        //Sinalizando a saida da fila
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    //Irá sair da fila somente se o consulmidor sinalizar
                    channel.BasicConsume(
                                queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);

                    Console.WriteLine("Precione qualquer tecla para continuar");
                    Console.ReadLine();
                }
            }
        }
    }
}
