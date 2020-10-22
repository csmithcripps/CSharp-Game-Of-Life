using Display;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Life
{
    /// <summary>
    /// Top level running of Conway's life simulator
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            //Init Game of Life Objects
            Settings settings = ArgumentHandler.HandleArguments(args);
            Universe universe = SeedReader.HandleSeed(settings);
            Grid displayGrid = new Grid(settings.height, settings.width);
            UniverseMemory universeMemory = new UniverseMemory(settings.nGenerations, settings);
            int steadyStatePoint = -1;


            //Print Runtime Parameters
            settings.PrintParameters();

            //Wait to start
            Console.WriteLine("Press <SPACE> to start...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;

            //Run Simulation with Runtime Parameters
            displayGrid.InitializeWindow();
            Stopwatch watch = new Stopwatch();

            for (int iteration = 0; iteration <= settings.nGenerations; iteration++)
            {
                watch.Restart();

                // Set Footnote to the current iteration
                displayGrid.SetFootnote(string.Format(CultureInfo.InvariantCulture,
                                                "Iteration: {0,3}", iteration));

                // Draw current state to the console window
                universe.Draw(displayGrid);
                displayGrid.Render();

                // Update to next generation
                universe.Update();

                // Check for steady state
                steadyStatePoint = universeMemory.Search(universe);

                if (steadyStatePoint != -1)
                {
                    break;
                }


                // Add current generation to memory (Using implicit conversion to CellStatus[,])
                universeMemory.Add(universe);

                // Wait until at least maximum update time is reached
                while (watch.ElapsedMilliseconds <= 1000 / settings.updateRate) ;

                // If step is enabled wait for spacebar to be pressed
                while (settings.step && Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            }

            // Prepare to exit
            displayGrid.SetFootnote("Press <Space> to Exit");
            displayGrid.Render();
            SeedWriter.WriteSeed(settings.outputFile, ref universe);

            // ait to exit
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;

            // Reset Console
            displayGrid.RevertWindow();
            Console.ResetColor();

            // Report outcome
            if (steadyStatePoint != -1)
            {
                Console.WriteLine("Steady State Detected");
                Console.WriteLine("Periodicity: {0}", (steadyStatePoint == 0) ?
                                                        "N/A" :
                                                        (steadyStatePoint + 1).ToString());
            }
        }
    }
}
