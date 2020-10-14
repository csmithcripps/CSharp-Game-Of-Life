using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    public class SeedReader
    {
        /// <summary>
        /// Initialises Universe universe with initial living cells
        /// </summary>
        /// 
        public static void HandleSeed(string filePath, ref Universe universe)
        {
            // If no seed is chosen, tell universe to randomise
            if (filePath == "N/A")
            {
                universe.RandomSeed();
            }
            else
            {
                using (TextReader reader = new StreamReader(filePath))
                {

                    // Flush first line
                    string line = reader.ReadLine();

                    if (line == "#version=1.0")
                    {
                        HandleVersion1(reader, ref universe);
                    }
                    else if (line == "#version=2.0")
                    {
                        HandleVersion2(reader, ref universe);
                    }
                }
            }
        }

        private static void HandleVersion1(TextReader reader, ref Universe universe)
        {
            int row, column;

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
                universe.SetCell(row, column, CellStates.Alive);

                // Read next line
                line = reader.ReadLine();
            }
        }
        

        private static void HandleVersion2(TextReader reader, ref Universe universe)
        { 
            // Initial Read
            string line = reader.ReadLine();

            // Read to end of file
            while (line != null)
            {
                // Take Cell from seed file
                line = line.Replace(",", "");
                string[] data = line.Split(" ");

                switch(data[1])
                {
                    case "cell:":
                        Structures.Cell.build(data, ref universe);
                        break;

                    default:
                        break;
                }

                // Read next line
                line = reader.ReadLine();
            }

        }
    }
}