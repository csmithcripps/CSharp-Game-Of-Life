using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    /// <summary>
    /// Top level running of Conway's life simulator
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            Settings settings = ArgumentHandler.HandleArguments(args);
            //Init Game of Life Class
            ConwaysLifeSimulator simulator = new ConwaysLifeSimulator(settings);
            
            //Print Runtime Parameters
            simulator.settings.PrintParameters();

            //Wait to start
            Console.WriteLine("Press <SPACE> to start...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;

            //Run Simulation with Runtime Parameters
            simulator.RunSimulation();

            //Wait to exit
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            
            //Reset Console
            simulator.exit();
            Console.ResetColor();
            
        }
    }
}
