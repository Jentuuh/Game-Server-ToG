using System;
using System.Threading;

namespace TowerOfGodServer
{
      /*
      * @author Jente Vandersanden
      * 18/08/2020
      * Main control class
      * 
      * Credits to Tom Weiland's tutorial
      */
    class Program
    {
        private static bool isRunning = false;
        static void Main(string[] args)
        {
            Console.Title = "Tower of God Server Software";
            isRunning = true;

            // Start main thread
            Thread main_thread = new Thread(new ThreadStart(MainThread));
            main_thread.Start();

            // Run the server
            Server.Start(32, 33592);
            
        }

        // Method that takes care of the main thread's running
        private static void MainThread()
        {
            Console.WriteLine($"Main Thread started. Running at {CONSTANTS.TICKS_PER_SEC} ticks per second.");

            DateTime next_loop = DateTime.Now;

            while (isRunning)
            {
                while(next_loop < DateTime.Now)
                {
                    GameLogic.Update();

                    next_loop = next_loop.AddMilliseconds(CONSTANTS.MS_PER_TICK);
                
                    if(next_loop > DateTime.Now)
                    {
                        Thread.Sleep(next_loop - DateTime.Now);
                    }
                }
            }
        }

    }
}
