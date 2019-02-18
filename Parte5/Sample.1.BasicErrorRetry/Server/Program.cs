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
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample7";

        

        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ queue processor");
            Console.WriteLine();
            Console.WriteLine();

            #region Connect to RabbitMQ
            DisplaySettings();

            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };            

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            #endregion

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
                    //Discard the message because it cant be processed
                    Console.WriteLine("Rejecting the message as it cant be processed - {0}", message);
                    model.BasicReject(deliveryArgs.DeliveryTag, false);
                }
                else
                {
                    //Reject the message so it can be retried - EG application error processing message
                    Console.WriteLine("Rejecting the message as there was an error processing it - {0}", message);
                    model.BasicReject(deliveryArgs.DeliveryTag, true);
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
