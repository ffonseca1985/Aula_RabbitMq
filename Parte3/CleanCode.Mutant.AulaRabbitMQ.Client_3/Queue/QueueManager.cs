using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCode.Mutant.AulaRabbitMQ.Client_3.Queue
{
    using RabbitMQ.Client;

    public class QueueManager
    {
        string _hostName = "localhost";
        string _queueName = "queue_mutant";
        string _exchange = "exchage_mutant";

        public void Send(string message) 
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName};

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    //manda para todas as filas que tem uma ligação com a 
                    channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: _exchange,
                        routingKey: "",
                        basicProperties: properties,
                        body: body);
                }
            }
        }
    }
}
