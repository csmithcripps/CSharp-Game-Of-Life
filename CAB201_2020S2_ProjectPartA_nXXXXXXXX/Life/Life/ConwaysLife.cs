using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{

    public class ConwaysLifeSimulator
    {
        private const int MinGenerations = 1;
        private const int MinSize = 4;
        private const int MaxSize = 48;
        private bool step = false;
        private bool periodic = false;
        private float randomFactor = 0.5f;
        private float updateRate = 5f;
        private int nGenerations = 50;
        private string seed = "N/A";
        private int height = 16;
        private int width = 16;
        private CellAutomata automata;
        public Grid displayGrid;

        public ConwaysLifeSimulator()
        {
            automata = new CellAutomata(height, width);
            SetupUniverse();
        }

        public ConwaysLifeSimulator(string[] args)
        {
            HandleInputArguments(args);
            automata = new CellAutomata(height, width, periodic);
            SetupUniverse();
        }

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


        private void SetupUniverse()
        {

            if (seed == "N/A")
            {
                automata.RandomSeed(randomFactor);
            }
            else
            {
                using (TextReader reader = new StreamReader(seed))
                {
                    int row, column;
                    reader.ReadLine();
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split(" ");
                        int.TryParse(data[0], out row);
                        int.TryParse(data[1], out column);
                        automata.SetCell(row, column, 1);

                        line = reader.ReadLine();
                    }
                }
            }
        }

        public void RunSimulation()
        {
            displayGrid = new Grid(height, width);
            displayGrid.InitializeWindow();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Stopwatch watch = new Stopwatch();

            for (int iteration = 0; iteration <= nGenerations; iteration++)
            {
                watch.Restart();

                displayGrid.SetFootnote(string.Format(CultureInfo.InvariantCulture,
                                                "Iteration: {0,3}", iteration));

                automata.Draw(displayGrid);
                // Render updates to the console window...
                displayGrid.Render();

                automata.Update();
                while (watch.ElapsedMilliseconds <= 1000 / updateRate) ;
                while (step && Console.ReadKey().Key != ConsoleKey.Spacebar) ;
            }
            displayGrid.SetFootnote("Press <Space> to Exit");
            displayGrid.Render();

        }

        public void exit()
        {
            displayGrid.RevertWindow();
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
                if (args[argumentNum][0] != '-')
                {
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
                        isNumber = float.TryParse(args[argumentNum + 1],
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
                        float inputUpdateRate = 5;
                        if (ParseNumberInRange(args[argumentNum + 1], ref inputUpdateRate, 0, 30))
                        {
                            this.updateRate = inputUpdateRate;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    case "--generations":
                        int inputGenerations = 50;
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

                    case "--seed":
                        string fileName;
                        fileName = args[argumentNum + 1];
                        if (File.Exists(fileName) && Path.GetExtension(fileName) == ".seed")
                        {
                            this.seed = fileName;
                        }
                        else
                        {
                            argumentSuccessful = false;
                        }
                        break;

                    case "--dimensions":
                        int inputRows = 20;
                        int inputColumns = 20;
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