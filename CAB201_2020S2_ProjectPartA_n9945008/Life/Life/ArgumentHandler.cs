using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;


namespace Life
{
    class ArgumentHandler
    {
        private const int MinGenerations = 1;
        private const int MinSize = 4;
        private const int MaxSize = 48;
        private const int MinRandomFactor = 0;
        private const int MaxRandomFactor = 1;
        private const int MinUpdate = 0;
        private const int MaxUpdate = 30;

        /// <summary>
        /// Handles the input console arguments by setting the object parameters
        /// </summary>
        /// <param name="args">Input arguments from console interface</param>
        public static Settings HandleArguments(string[] args)
        {
            IFormatProvider provider = CultureInfo.InvariantCulture;

            Settings settings = new Settings();
            bool argumentSuccessful = true;
            string defaultVal = "";

            // Run through each argument (including parameters)
            for (int argumentNum = 0; argumentNum < args.Length; argumentNum++)
            {
                try
                {
                    // Do some action based on which flag is found
                    switch (args[argumentNum])
                    {
                        // Activate step mode
                        case "--step":
                            defaultVal = settings.step.ToString();
                            settings.step = true;
                            break;

                        // Activate boundary conditions
                        case "--periodic":
                            defaultVal = settings.periodic.ToString();
                            settings.periodic = true;
                            break;

                        // Set the random factor
                        case "--random":
                            defaultVal = settings.randomFactor.ToString();
                            ParseArgument(ref settings.randomFactor, args[argumentNum + 1], MinRandomFactor, MaxRandomFactor);
                            break;

                        // Set the maximum update rate
                        case "--max-update":
                            defaultVal = settings.updateRate.ToString();
                            ParseArgument(ref settings.updateRate, args[argumentNum + 1], MinUpdate, MaxUpdate);
                            break;

                        // Set the number of generations
                        case "--generations":
                            defaultVal = settings.nGenerations.ToString();
                            ParseArgument(ref settings.nGenerations, args[argumentNum + 1], MinGenerations);
                            break;

                        // Set the seed
                        case "--seed":
                            defaultVal = settings.seed.ToString();
                            ParseArgument(ref settings.seed, args[argumentNum + 1]);
                            break;

                        // Set the dimensions
                        case "--dimensions":
                            defaultVal = settings.width.ToString() + "x" + settings.height.ToString();
                            ParseArgument(ref settings.width, args[argumentNum + 1], MinSize, MaxSize);
                            ParseArgument(ref settings.height, args[argumentNum + 1], MinSize, MaxSize);
                            break;

                        default:
                            break;
                    }
                }
                catch(ArgumentException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unsuccessfully Changed " + args[argumentNum]);
                    Console.WriteLine(e.Message);
                    Console.Write("Defaulting To ");
                    Console.WriteLine(defaultVal);
                    Console.ResetColor();
                }
            }

            return settings;
        }

        private static void ParseArgument(ref float arg, string param, double min = Double.NegativeInfinity,
                                    double max = Double.PositiveInfinity)
        {
            float inputArg = 0f;

            // Check that the parameter is in range and assign it
            if (ParseNumberInRange(param, ref inputArg, min, max))
            {
                arg = inputArg;
            }
            else
            {
                throw new System.ArgumentException("Some Arguments Entered Incorrectly", param);
            }
        }

        private static void ParseArgument(ref int arg, string param, double min = Double.NegativeInfinity,
                                    double max = Double.PositiveInfinity)
        {
            int inputArg = 0;

            // Check that the parameter is in range and assign it
            if (ParseNumberInRange(param, ref inputArg, min, max))
            {
                arg = inputArg;
            }
            else
            {
                throw new System.ArgumentException("Some Arguments Entered Incorrectly", param);
            }
        }

        private static void ParseArgument(ref string arg, string param)
        {

            // Check that the parameter is in range and assign it
            if (File.Exists(param) && Path.GetExtension(param) == ".seed")
            {
                arg = param;
            }
            else
            {
                throw new System.ArgumentException("Some Arguments Entered Incorrectly", param);
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
        private static bool  ParseNumberInRange(string numberToParse, ref int outputNumber, double min = Double.NegativeInfinity,
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
        private static bool ParseNumberInRange(string numberToParse, ref float outputNumber, double min = Double.NegativeInfinity,
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
    }
}
