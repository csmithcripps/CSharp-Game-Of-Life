using System;
using System.IO;
using System.Collections.Generic;

namespace Life
{
    public static class SeedReader
    {
        /// <summary>
        ///     Construct a universe object based on the seed information
        /// </summary>
        /// <param name="settings">
        ///     System settings (contains seed)
        ///     also needed to initialise Universe
        /// </param>
        public static Universe HandleSeed(Settings settings)
        {
            Universe universe = new Universe(settings);
            string filePath = settings.seed;

            try
            {
                using TextReader reader = new StreamReader(filePath);
                // Initial Read
                string line = reader.ReadLine();

                universe = line switch
                {
                    "#version=1.0" => HandleVersion1(reader, universe),
                    "#version=2.0" => HandleVersion2(reader, universe),
                    _ => RandomSeed(universe),
                };
            }
            // This should only be the case if the seed was set to N/A
            catch (System.IO.DirectoryNotFoundException)
            {
                universe = RandomSeed(universe);
            }

            // This will occur if the seed is invalid for the universe dimensions.
            catch (System.IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error in Seed (Tried to set cell outside of dimensions) \nDefaulting to Random");
                universe = RandomSeed(universe);
                Console.ResetColor();
            }

            return universe;
        }


        /// <summary>
        /// Randomises the universe based on some factor of randomness
        /// </summary>
        /// <param name="randomFactor">Percentage of cells that will be alive</param>
        ///     
        public static Universe RandomSeed(Universe universe)
        {
            Random generator = new Random();
            Settings settings = universe.Settings;

            // Run through every row and column
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {
                    // If some random number is less than the random factor, set cell to alive
                    //      else set cell to dead
                    universe[row, column] = (generator.NextDouble() < settings.randomFactor) ?
                        CellStatus.Alive : CellStatus.Dead;
                }
            }
            return universe;
        }

        /// <summary>
        ///     Handles version 1 seeds.
        /// </summary>
        /// <param name="reader">
        ///     A textreader object attached to a seed 
        /// </param>
        /// <param name="universe">
        ///     The universe object to initialise from the seed
        /// </param>
        private static Universe HandleVersion1(TextReader reader, Universe universe)
        {

            // Initial Read
            string line = reader.ReadLine();

            // Read to end of file
            while (line != null)
            {
                // Take Cell from seed file
                string[] data = line.Split(" ");
                int.TryParse(data[0], out int row);
                int.TryParse(data[1], out int column);

                // Set chosen cell to alive
                universe[row, column] = CellStatus.Alive;

                // Read next line
                line = reader.ReadLine();
            }
            return universe;
        }

        /// <summary>
        ///     Handles version 2 seeds.
        /// </summary>
        /// <param name="reader">
        ///     A textreader object attached to a seed 
        /// </param>
        /// <param name="universe">
        ///     The universe object to initialise from the seed
        /// </param>
        private static Universe HandleVersion2(TextReader reader, Universe universe)
        {
            // Initial Read
            string line = reader.ReadLine();

            // Read to end of file
            while (line != null)
            {
                // Trim unrelated information
                line = line.Replace(",", "");
                line = line.Replace(":", "");
                Console.WriteLine(line);
                string[] data = line.Split(" ");
                data = RemoveEmpty(data);

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
            return universe;
        }

        /// <summary>
        ///     Remove empty elements in a string array
        /// </summary>
        /// <param name="array">
        ///     A string array
        /// </param>
        /// <returns>
        ///     The inputted string array with empty elements removed.
        /// </returns>
        private static string[] RemoveEmpty(string[] array)
        {
            var temp = new List<string>();
            foreach (string s in array)
            {
                if (!string.IsNullOrEmpty(s))
                    temp.Add(s);
            }
            return temp.ToArray();
        }
    }
}