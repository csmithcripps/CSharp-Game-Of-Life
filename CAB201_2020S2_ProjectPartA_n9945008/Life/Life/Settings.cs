using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;


namespace Life
{
    public class Settings
    {
        public const int MinGenerations = 1;
        public const int MinSize = 4;
        public const int MaxSize = 48;
        public bool step = false;
        public bool periodic = false;
        public float randomFactor = 0.5f;
        public float updateRate = 5f;
        public int nGenerations = 50;
        public string seed = "N/A";
        public int height = 16;
        public int width = 16;

        public Settings()
        {
        }

        public Settings(string[] args)
        {
            HandleInputArguments(args);
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
        /// Handles the input console arguments by setting the object parameters
        /// </summary>
        /// <param name="args">Input arguments from console interface</param>
        private void HandleInputArguments(string[] args)
        {
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

                    // Activate step mode
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
