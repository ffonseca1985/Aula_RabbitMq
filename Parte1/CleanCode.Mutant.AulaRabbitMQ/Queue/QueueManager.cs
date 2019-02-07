using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCode.Mutant.AulaRabbitMQ.Client.Queue
{
    using RabbitMQ.Client;

    public class QueueManager
    {
        string _hostName = "localhost";
        string _queueName = "queue_mutant";

        public void Send(string message) 
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName};

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //Enviar no máximo 1 por vez
                    channel.BasicQos(0, 1, false);
                    channel.QueueDeclare(_queueName, true, false, false, null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _queueName,
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
