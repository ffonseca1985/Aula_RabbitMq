using CleanCode.Mutant.AulaRabbitMQ.Client_2.Queue;
using System;

namespace CleanCode.Mutant.AulaRabbitMQ.Client_2
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
