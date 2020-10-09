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
        private Universe universe;
        public Settings settings { get; private set; }
        public Grid displayGrid;

        /// <summary>
        /// Constructs new simulator with all default values
        /// 
        public ConwaysLifeSimulator()
        {
            settings = new Settings();
            universe = new Universe(settings);
            HandleSeed();
        }
        
        public ConwaysLifeSimulator(Settings ParsedSettings)
        {
            this.settings = ParsedSettings;
            universe = new Universe(settings);
            HandleSeed();
        }

        /// <summary>
        /// Initialises Universe universe with initial living cells
        /// </summary>
        /// 
        private void HandleSeed()
        {
            // If no seed is chosen, tell universe to randomise
            if (settings.seed == "N/A")
            {
                universe.RandomSeed(settings.randomFactor);
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
                        universe.SetCellAlive(row, column);

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
                universe.Draw(displayGrid);
                displayGrid.Render();

                // Update to next generation
                universe.Update();

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