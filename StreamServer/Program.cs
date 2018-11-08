
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Silo Host");

            Task.WaitAll(Server.Instance.InitAsync());

            Console.ReadLine();
        }
    }
}
