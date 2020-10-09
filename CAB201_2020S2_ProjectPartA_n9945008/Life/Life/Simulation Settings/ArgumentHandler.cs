using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;


namespace Life
{
    public class ArgumentHandler
    {
        /// <summary>
        /// Handles the input console arguments by setting the object parameters
        /// </summary>
        /// <param name="args">Input arguments from console interface</param>
        public static Settings HandleArguments(string[] args)
        {
            IFormatProvider provider = CultureInfo.InvariantCulture;

            Settings settings = new Settings();
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
                            settings.step = (BoolOption)true;
                            break;

                        // Activate boundary conditions
                        case "--periodic":
                            defaultVal = settings.periodic.ToString();
                            settings.periodic = (BoolOption)true;
                            break;

                        // Set the random factor
                        case "--random":
                            defaultVal = settings.randomFactor.ToString();
                            settings.randomFactor.Set(args[argumentNum + 1]);
                            break;

                        // Set the maximum update rate
                        case "--max-update":
                            defaultVal = settings.updateRate.ToString();
                            settings.updateRate.Set(args[argumentNum + 1]);
                            break;

                        // Set the number of generations
                        case "--generations":
                            defaultVal = settings.nGenerations.ToString();
                            settings.nGenerations.Set(args[argumentNum + 1]);
                            break;

                        // Set the seed
                        case "--seed":
                            defaultVal = settings.seed.ToString();
                            settings.seed.Set(args[argumentNum + 1]);
                            break;

                        // Set the dimensions
                        case "--dimensions":
                            defaultVal = settings.width.ToString() + "x" + settings.height.ToString();
                            settings.width.Set(args[argumentNum + 1]);
                            settings.height.Set(args[argumentNum + 1]);
                            break;

                        default:
                            break;
                    }
                }
                catch (ArgumentException e)
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
    }
}
