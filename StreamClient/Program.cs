using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client");

            Client.Instance.InitAsync().GetAwaiter().GetResult();

            Console.ReadLine();
        }
    }
}
