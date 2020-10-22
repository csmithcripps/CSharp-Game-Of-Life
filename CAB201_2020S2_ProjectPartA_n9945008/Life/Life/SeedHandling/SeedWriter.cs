using System;
using System.IO;
using System.Collections.Generic;

namespace Life
{
    public class SeedWriter
    {
        /// <summary>
        /// Initialises Universe universe with initial living cells
        /// </summary>
        /// 
        public static void WriteSeed(string filePath, ref Universe universe)
        {
            if (filePath == "N/A") return;

            List<string> seedV2 = new List<string>();
            seedV2.Add("#version=2.0");

            try
            {
                for (int row = 0; row < universe.settings.height; row++)
                {
                    for (int column = 0; column < universe.settings.width; column++)
                    {
                        if (universe.GetCell(row, column) == CellStatus.Alive)
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