using System;

namespace TowerOfGodServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tower of God Server Software";

            // Run the server
            Server.Start(32, 33592);

            Console.ReadKey();
        }
    }
}
