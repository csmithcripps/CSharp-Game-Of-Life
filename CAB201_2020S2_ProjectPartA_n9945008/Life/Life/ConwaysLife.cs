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
        private const int MinGenerations = 1;
        private const int MinSize = 4;
        private const int MaxSize = 48;
        private bool step = false;
        private bool periodic = false;
        private float randomFactor = 0.5f;
        private float updateRate = 5f;
        private int nGenerations = 50;
        private string seed = "N/A";
        private int height = 16;
        private int width = 16;
        private CellAutomata automata;
        public Grid displayGrid;

        /// <summary>
        /// Constructs new simulator with all default values
        /// 
        public ConwaysLifeSimulator()
        {
            automata = new CellAutomata(height, width);
            HandleSeed();
        }

        /// <summary>
        /// Constructs new simulator with values based on console flags
        /// </summary>
        /// <param name="args">A set of console arguments</param>
        /// 
        public ConwaysLifeSimulator(string[] args)
        {
            HandleInputArguments(args);
            automata = new CellAutomata(height, width, periodic);
            HandleSeed();
        }

        /// <summary>
        /// Prints the current value of each of the simulator parameters.
        /// </summary>
        /// 
        public void PrintParameters()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Chosen Settings");
            WriteParameter("Parameter", "Value");
            WriteParameter("Step Mode", this.step);
            WriteParameter("Periodic", this.periodic);
            WriteParameter("Random Factor", this.randomFactor);
            WriteParameter("Maximum Update Rate", this.updateRate);
            WriteParameter("Generations", this.nGenerations);
            WriteParameter("Seed", this.seed);
            WriteParameter("Rows", this.height);
            WriteParameter("Columns", this.width);
        }

        /// <summary>
        /// Initialises cellautomata universe with initial living cells
        /// </summary>
        /// 
        private void HandleSeed()
        {
            // If no seed is chosen, tell automata to randomise
            if (seed == "N/A")
            {
                automata.RandomSeed(randomFactor);
            }
            else
            {
                using (TextReader reader = new StreamReader(seed))
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
            displayGrid = new Grid(height, width);
            displayGrid.InitializeWindow();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Stopwatch watch = new Stopwatch();

            for (int iteration = 0; iteration <= nGenerations; iteration++)
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
                while (watch.ElapsedMilliseconds <= 1000 / updateRate) ;

                // If step is enabled wait for spacebar to be pressed
                while (step && Console.ReadKey().Key != ConsoleKey.Spacebar) ;
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



        /// <summary>
        /// Handles the input console arguments by setting the object parameters
        /// </summary>
        /// <param name="args">Input arguments from console interface</param>
        private void HandleInputArguments(string[] args)
        {
            bool isNumber;
            bool argumentSuccessful = true;
            IFormatProvider provider = CultureInfo.InvariantCulture;

            // Run through each argument (including parameters)
            for (int argumentNum = 0; argumentNum < args.Length; argumentNum++)
            {

                // Skip if this argument is not a flag
                if (args[argumentNum][0] != '-')
                {
                    continue;
                }

                // Do some action based on which flag is found
                switch (args[argumentNum])
                {

                    // Activate steo mode
                    case "--step":
                        this.step = true;
                        break;

                    // Activate boundary conditions
                    case "--periodic":
                        this.periodic = true;
                        break;

                    // Set the random factor
                    case "--random":
                        float inputRandomFactor = randomFactor;

                        // Check that the parameter is in range and assign it
                        if (ParseNumberInRange(args[argumentNum + 1], ref inputRandomFactor, 0, 1))
                        {
                            this.randomFactor = inputRandomFactor;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    // Set the maximum update rate
                    case "--max-update":
                        float inputUpdateRate = 5;

                        // Check that the parameter is in range and assign it
                        if (ParseNumberInRange(args[argumentNum + 1], ref inputUpdateRate, 0, 30))
                        {
                            this.updateRate = inputUpdateRate;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    // Set the number of generations
                    case "--generations":
                        int inputGenerations = 50;

                        // Check that the parameter is in range and assign it
                        if (ParseNumberInRange(args[argumentNum + 1],
                                            ref inputGenerations, MinGenerations))
                        {
                            this.nGenerations = inputGenerations;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    // Set the seed
                    case "--seed":
                        string fileName;
                        fileName = args[argumentNum + 1];

                        // Check that this is a valid file with the file extension .seed
                        if (File.Exists(fileName) && Path.GetExtension(fileName) == ".seed")
                        {
                            this.seed = fileName;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    // Set the dimensions
                    case "--dimensions":
                        int inputRows = 20;
                        int inputColumns = 20;

                        // Check that the parameters are in range and assign them
                        if (ParseNumberInRange(args[argumentNum + 1], ref inputRows, MinSize, MaxSize) &&
                            ParseNumberInRange(args[argumentNum + 2], ref inputColumns, MinSize, MaxSize))
                        {
                            this.width = inputRows;
                            this.height = inputColumns;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    default:
                        break;
                }
            }
            if (!argumentSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Some Arguments Entered Incorrectly");
            }
        }

        /// <summary>
        /// Try to parse an integer number from some text, with a set range allowed
        /// </summary>
        /// <param name="numberToParse">Input string to parse</param>
        /// <param name="outputNumber">Where to store the resulting integer</param>
        /// <param name="min">the minimum point of the range</param>
        /// <param name="max">The maximum point of the range</param>
        /// <returns>True if the string is a number and it is within the specified range</returns>
        /// 
        private bool ParseNumberInRange(string numberToParse, ref int outputNumber, double min = Double.NegativeInfinity,
                                    double max = Double.PositiveInfinity)
        {
            bool isNumber = int.TryParse(numberToParse,
                                    NumberStyles.Integer |
                                    NumberStyles.AllowDecimalPoint |
                                    NumberStyles.AllowThousands,
                                    CultureInfo.InvariantCulture,
                                    out outputNumber);
            return (isNumber && (outputNumber >= min) && (outputNumber <= max));
        }

        /// <summary>
        /// Try to parse an floating point number from some text, with a set range allowed
        /// </summary>
        /// <param name="numberToParse">Input string to parse</param>
        /// <param name="outputNumber">Where to store the resulting float</param>
        /// <param name="min">the minimum point of the range</param>
        /// <param name="max">The maximum point of the range</param>
        /// <returns>True if the string is a number and it is within the specified range</returns>
        /// 
        private bool ParseNumberInRange(string numberToParse, ref float outputNumber, double min = Double.NegativeInfinity,
                                    double max = Double.PositiveInfinity)
        {
            bool isNumber = float.TryParse(numberToParse,
                                    NumberStyles.Integer |
                                    NumberStyles.AllowDecimalPoint |
                                    NumberStyles.AllowThousands,
                                    CultureInfo.InvariantCulture,
                                    out outputNumber);
            return (isNumber && (outputNumber >= min) && (outputNumber <= max));
        }

        /// <summary>
        /// Write parameter value to console in an attractive way
        /// </summary>
        /// <param name="name">name of the parameter</param>
        /// <param name="arg">value of the parameter as an unspecified object</param>
        /// 
        private void WriteParameter<T>(string name, T arg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0, 20}:\t", name);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("{0}\n", arg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}