using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenAlg___Coin
{
    class Program
    {
        /*
         * Main function of the program.  It creates a new object, runs, then pauses before 
         * closing the program
         */
        static void Main(string[] args)
        {
            GenAlg list = new GenAlg();
            list.run();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
