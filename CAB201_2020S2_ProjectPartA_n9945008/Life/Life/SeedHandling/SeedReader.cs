using System;
using System.IO;

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
            try
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
            catch (System.IO.DirectoryNotFoundException)
            {
                RandomSeed(ref universe);
            }
            catch (System.IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error in Seed (Tried to set cell outside of dimensions) \nDefaulting to Random");
                RandomSeed(ref universe);
                Console.ResetColor();
            }
        }


        /// <summary>
        /// Randomises the universe based on some factor of randomness
        /// </summary>
        /// <param name="randomFactor">Percentage of cells that will be alive</param>
        ///     
        public static void RandomSeed(ref Universe universe)
        {
            Random generator = new Random();
            Settings settings = universe.settings;

            // Run through every row and column
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {
                    // If some random number is less than the random factor, set cell to alive
                    //      else set cell to dead
                    universe.SetCell(row, column, (generator.NextDouble() < settings.randomFactor) ?
                        CellStatus.Alive : CellStatus.Dead);
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
                universe.SetCell(row, column, CellStatus.Alive);

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
                line = line.Replace(":", "");
                Console.WriteLine(line);
                string[] data = line.Split(" ");

                switch (data[1])
                {
                    case "cell":
                        new Structures.Cell(data, ref universe);
                        break;

                    case "rectangle":
                        new Structures.Rectangle(data, ref universe);
                        break;

                    case "ellipse":
                        new Structures.Ellipse(data, ref universe);
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