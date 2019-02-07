using System;
using CleanCode.Mutant.AulaRabbitMQ.Client.Queue;
using RabbitMQ.Client;

namespace CleanCode.Mutant.AulaRabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new QueueManager();

            while (true)
            {
                Console.WriteLine("Dispare uma mensgam!!!!");
                var message = Console.ReadLine();
                manager.Send(message);
            }
        }
    }
}
