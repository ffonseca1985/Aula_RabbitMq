using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCode.Mutant.AulaRabbitMQ.Client_4.Queue
{
    using RabbitMQ.Client;

    public class QueueManager
    {
        string _hostName = "localhost";
        string _exchange = "exchage_mutant_direct";
        string _key = "key_mutant";

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
                    channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Direct);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: _exchange,
                        routingKey: _key,
                        basicProperties: properties,
                        body: body);
                }
            }
        }
    }
}
