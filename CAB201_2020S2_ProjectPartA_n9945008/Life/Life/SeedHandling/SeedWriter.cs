using System;
using System.IO;
using System.Collections.Generic;

namespace Life
{/// <summary>
/// <classSeedWriter</class>
/// Static class used to write to seed files
/// </summary>
    public static class SeedWriter
    {
        /// <summary>
        /// Writes the final version of the universe to a version 2 seed file
        /// </summary>
        /// <param name="filePath"> 
        ///     The path to write to
        /// </param>
        /// <param name="universe">
        ///     The universe to write to file
        /// </param>
        /// 
        public static void WriteSeed(string filePath, ref Universe universe)
        {
            if (filePath == "N/A") return;

            List<string> seedV2 = new List<string>();
            seedV2.Add("#version=2.0");

            try
            {
                for (int row = 0; row < universe.Settings.height; row++)
                {
                    for (int column = 0; column < universe.Settings.width; column++)
                    {
                        if (universe[row, column] == CellStatus.Alive)
                        {
                            seedV2.Add($"(o) cell: {row}, {column}");
                        }
                    }

                }

                File.WriteAllLines(filePath, seedV2);
            }
            catch (System.IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Output Failed");
                Console.ResetColor();
            }
        }

    }
}