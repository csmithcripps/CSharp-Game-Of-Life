using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;


namespace Life
{
    public static class ArgumentHandler
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
                    List<string> parameterList = new List<string>();
                    int cursor = argumentNum + 1;
                    while (cursor < args.Length)
                    {
                        if (args[cursor][0] == '-')
                        {
                            break;
                        }
                        parameterList.Add(args[cursor]);
                        cursor++;
                    }
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
                            settings.randomFactor.Set(parameterList[0]);
                            break;

                        // Set the maximum update rate
                        case "--max-update":
                            defaultVal = settings.updateRate.ToString();
                            settings.updateRate.Set(parameterList[0]);
                            break;

                        // Set the number of generations
                        case "--generations":
                            defaultVal = settings.nGenerations.ToString();
                            settings.nGenerations.Set(parameterList[0]);
                            break;

                        // Set the seed
                        case "--seed":
                            defaultVal = settings.seed.ToString();
                            if (!File.Exists(parameterList[0]))
                            {
                                throw new System.ArgumentException("Some Arguments Entered Incorrectly",
                                                                    parameterList[0]);
                            }
                            settings.seed.Set(parameterList[0]);
                            break;

                        case "--output":
                            defaultVal = settings.outputFile.ToString();
                            settings.outputFile.Set(parameterList[0]);
                            break;

                        // Set the dimensions
                        case "--dimensions":
                            defaultVal = settings.width.ToString() + "x" + settings.height.ToString();
                            settings.width.Set(parameterList[0]);
                            settings.height.Set(parameterList[1]);
                            break;

                        case "--survival":
                            defaultVal = settings.survival.ToString();
                            settings.survival.Set(parameterList.ToArray());
                            break;

                        case "--birth":
                            defaultVal = settings.birth.ToString();
                            settings.birth.Set(parameterList.ToArray());
                            break;

                        case "--neighbour":
                            defaultVal = settings.neighborhoodStyle.ToString();
                            settings.neighborhoodStyle.Set(parameterList[0]);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
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
