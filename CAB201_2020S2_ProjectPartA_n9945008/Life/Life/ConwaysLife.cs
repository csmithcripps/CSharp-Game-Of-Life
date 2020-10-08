using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    /// <summary>
    /// A class which can be used to run a simulation of Conway's game of life. 
    /// Contains the basic program flow and display code.
    /// </summary>
    public class ConwaysLifeSimulator
    {
        private CellAutomata automata;
        public Settings settings { get; private set; }
        public Grid displayGrid;

        /// <summary>
        /// Constructs new simulator with all default values
        /// 
        public ConwaysLifeSimulator()
        {
            settings = new Settings();
            automata = new CellAutomata(settings);
            HandleSeed();
        }

        /// <summary>
        /// Constructs new simulator with values based on console flags
        /// </summary>
        /// <param name="args">A set of console arguments</param>
        /// 
        public ConwaysLifeSimulator(string[] args)
        {
            settings = new Settings(args);
            automata = new CellAutomata(settings);
            HandleSeed();
        }

        /// <summary>
        /// Initialises cellautomata universe with initial living cells
        /// </summary>
        /// 
        private void HandleSeed()
        {
            // If no seed is chosen, tell automata to randomise
            if (settings.seed == "N/A")
            {
                automata.RandomSeed(settings.randomFactor);
            }
            else
            {
                using (TextReader reader = new StreamReader(settings.seed))
                {
                    int row, column;

                    // Flush first line
                    reader.ReadLine();

                    // Initial Read
                    string line = reader.ReadLine();

                    // Read to end of file
                    while (line != null)
                    {
                        // Take Cell from seed file
                        string[] data = line.Split(" ");
                        int.TryParse(data[0], out row);
                        int.TryParse(data[1], out column);

                        // Set chosen cell to alive
                        automata.SetCellAlive(row, column);

                        // Read next line
                        line = reader.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Run the actual simulation (iterate through generations at desired speed) 
        /// </summary>
        public void RunSimulation()
        {
            displayGrid = new Grid(settings.height, settings.width);
            displayGrid.InitializeWindow();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Stopwatch watch = new Stopwatch();

            for (int iteration = 0; iteration <= settings.nGenerations; iteration++)
            {
                watch.Restart();

                // Set Footnote to the current iteration
                displayGrid.SetFootnote(string.Format(CultureInfo.InvariantCulture,
                                                "Iteration: {0,3}", iteration));

                // Draw current state to the console window
                automata.Draw(displayGrid);
                displayGrid.Render();

                // Update to next generation
                automata.Update();

                // Wait until at least maximum update time is reached
                while (watch.ElapsedMilliseconds <= 1000 / settings.updateRate) ;

                // If step is enabled wait for spacebar to be pressed
                while (settings.step && Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            }
            displayGrid.SetFootnote("Press <Space> to Exit");
            displayGrid.Render();

        }

        /// <summary>
        /// Exit simulation
        /// </summary>
        public void exit()
        {
            displayGrid.RevertWindow();
        }
    }
}