using CleanCode.Mutant.AulaRabbitMQ.Server_3.Queue;
using System;

namespace CleanCode.Mutant.AulaRabbitMQ.Server_3
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
