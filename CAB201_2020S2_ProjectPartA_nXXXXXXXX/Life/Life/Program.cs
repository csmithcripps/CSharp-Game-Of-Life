using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{

    public class ConwaysLife
    {
        public bool step;
        public bool periodic;
        public float randomFactor;
        public float updateRate;
        public int nGenerations;
        public String seed;
        public int height;
        public int width;

        public ConwaysLife(string[] args)
        {
            step = false;
            periodic = false;
            randomFactor = 0.5f;
            updateRate = 5f;
            nGenerations = 50;
            seed = "N/A";
            height = 16;
            width = 16;
            if (args.Length != 0)
            {
                HandleInputArguments(args);
            }
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

            for (int argumentNum = 0; argumentNum < args.Length; argumentNum++)
            {
                if (args[argumentNum][0] != '-'){
                    continue;
                }
                switch (args[argumentNum])
                {
                    case "--step":
                        this.step = true;
                        break;

                    case "--periodic":
                        this.periodic = true;
                        break;

                    case "--random":
                        float inputRandomFactor;
                        isNumber = float.TryParse(args[argumentNum+1],
                                        NumberStyles.Integer |
                                        NumberStyles.AllowDecimalPoint |
                                        NumberStyles.AllowThousands,
                                        provider,
                                        out inputRandomFactor);
                        if (!isNumber || inputRandomFactor > 1 || inputRandomFactor < 0)
                        {
                            argumentSuccessful = false;
                        }
                        else
                        {
                            this.randomFactor = inputRandomFactor;
                        }
                        break;

                    case "--max-update":
                        float inputUpdateRate;
                        isNumber = float.TryParse(args[argumentNum+1],
                                        NumberStyles.Integer |
                                        NumberStyles.AllowDecimalPoint |
                                        NumberStyles.AllowThousands,
                                        provider,
                                        out inputUpdateRate);
                        if (!isNumber || inputUpdateRate > 30 || inputUpdateRate < 0)
                        {
                            argumentSuccessful = false;
                        }
                        else
                        {
                            this.updateRate = inputUpdateRate;
                        }
                        break;

                    case "--generations":
                        int inputGenerations;
                        isNumber = int.TryParse(args[argumentNum+1],
                                        NumberStyles.Integer |
                                        NumberStyles.AllowDecimalPoint |
                                        NumberStyles.AllowThousands,
                                        provider,
                                        out inputGenerations);
                        if (!isNumber || inputGenerations > 30 || inputGenerations < 0)
                        {
                            argumentSuccessful = false;
                        }
                        else
                        {
                            this.nGenerations = inputGenerations;
                        }
                        break;

                    case "--seed":
                        string fileName;
                        fileName = args[argumentNum+1];
                        if 
                        break;

                    case "--dimensions":
                        break;

                    default:
                        break;
                }
            }
            if (!argumentSuccessful)
            {
                Console.WriteLine("Some Arguments Entered Incorrectly");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // The following is just an example of how to use the Display.Grid class
            // Think of it as a "Hello World" program for using this small API
            // If it works correctly, it should display a little smiley face cell
            // by cell. The program will end after you press any key.

            // Feel free to remove or modify it when you are comfortable with it.

            //Init Game of Life Class
            ConwaysLife sim = new ConwaysLife(args);

            // Specify grid dimensions and active cells...
            int rows = 7, columns = 9;
            int[,] cells = {
                { 5, 3 },
                { 4, 3 },
                { 5, 5 },
                { 4, 5 },
                { 2, 1 },
                { 1, 2 },
                { 1, 3 },
                { 1, 4 },
                { 1, 5 },
                { 1, 6 },
                { 2, 7 }
            };

            // Construct grid...
            Grid grid = new Grid(rows, columns);

            // Wait for user to press a key...
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            // Initialize the grid window (this will resize the window and buffer)
            grid.InitializeWindow();

            // Set the footnote (appears in the bottom left of the screen).
            grid.SetFootnote("Smiley");

            Stopwatch watch = new Stopwatch();

            // For each of the cells...
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                watch.Restart();
                
                // Update grid with a new cell...
                grid.UpdateCell(cells[i, 0], cells[i, 1], CellState.Full);

                // Render updates to the console window...
                grid.Render();
                
                while (watch.ElapsedMilliseconds < 200) ;
            }

            // Set complete marker as true
            grid.IsComplete = true;

            // Render updates to the console window (grid should now display COMPLETE)...
            grid.Render();

            // Wait for user to press a key...
            Console.ReadKey();

            // Revert grid window size and buffer to normal
            grid.RevertWindow();
        }
    }
}
