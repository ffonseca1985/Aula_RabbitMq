using CleanCode.Mutant.AulaRabbitMQ.Server.Queue;
using System;

namespace CleanCode.Mutant.AulaRabbitMQ.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var manager = new QueueManager();
                manager.Receiver();
            }
        }
    }
}
