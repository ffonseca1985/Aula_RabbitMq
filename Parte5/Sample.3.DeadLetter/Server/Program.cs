using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private const string RetryHeader = "RETRY-COUNT";
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample9.Normal";

        

        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ queue processor");
            Console.WriteLine();
            Console.WriteLine();
            DisplaySettings();

            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };            

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            model.BasicQos(0, 1, false);
            var consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(QueueName, false, consumer);

            while (true)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Serialize message
                var message = Encoding.Default.GetString(deliveryArgs.Body);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Message Recieved - {0}", message);

                if (message == "1")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Acknowledging successful processing of message");
                    Console.ForegroundColor = ConsoleColor.White;
                    model.BasicAck(deliveryArgs.DeliveryTag, false);
                }                
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Message is <> 1 so rejecting and not requeuing message");
                    Console.ForegroundColor = ConsoleColor.White;

                    model.BasicReject(deliveryArgs.DeliveryTag, false);
                }
            }
        }        
        /// <summary>
        /// Displays the rabbit settings
        /// </summary>
        private static void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", HostName);
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", Password);
            Console.WriteLine("QueueName: {0}", QueueName);
        }
    }
}
