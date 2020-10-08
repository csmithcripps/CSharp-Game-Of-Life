using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    /// <summary>
    /// Top level running of Conway's life sim
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Init Game of Life Class
            ConwaysLifeSimulator sim = (args.Length != 0) ?
                    new ConwaysLifeSimulator(args) : new ConwaysLifeSimulator();
            
            //Print Runtime Parameters
            sim.PrintParameters();

            //Wait to start
            Console.WriteLine("Press <SPACE> to start...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;

            //Run Simulation with Runtime Parameters
            sim.RunSimulation();

            //Wait to exit
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            
            //Reset Console
            sim.exit();
            Console.ResetColor();
            
        }
    }
}
