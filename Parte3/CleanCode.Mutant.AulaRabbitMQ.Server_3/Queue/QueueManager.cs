﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CleanCode.Mutant.AulaRabbitMQ.Server_3.Queue
{
    public class QueueManager
    {
        string _hostName = "localhost";
        string _queueName = "queue_mutant";
        string _exchange = "exchage_mutant_fanout";

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
                    channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

                    _queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(
                              queue: _queueName,
                              exchange: _exchange,
                              routingKey: "");


                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    //Irá sair da fila somente se o consulmidor sinalizar
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
