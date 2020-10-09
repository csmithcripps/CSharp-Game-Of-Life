using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;


namespace Life
{
    public class Settings
    {
        private const int MinGenerations = 1;
        private const int MinSize = 4;
        private const int MaxSize = 48;
        private const int MinRandomFactor = 0;
        private const int MaxRandomFactor = 1;
        private const int MinUpdate = 0;
        private const int MaxUpdate = 30;
        public BoolOption step = new BoolOption("Step", false);
        public BoolOption periodic = new BoolOption("Periodic", false);
        public FloatOption randomFactor = new FloatOption("Random Factor", 
                                                            0.5f, 
                                                            MinRandomFactor, 
                                                            MaxRandomFactor);
        public FloatOption updateRate = new FloatOption("Update Rate", 
                                                            5f, 
                                                            MinUpdate, 
                                                            MaxUpdate);
        public IntOption nGenerations = new IntOption("Max Generations", 
                                                            50, 
                                                            MinGenerations);
        public IntOption height = new IntOption("Height", 16, MinSize, MaxSize);
        public IntOption width = new IntOption("Width", 16, MinSize, MaxSize);
        public FileOption seed = new FileOption("Seed", "N/A");

        public Settings()
        {
        }

        /// <summary>
        /// Prints the current value of each of the simulator parameters.
        /// </summary>
        /// 
        public void PrintParameters()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Chosen Settings");
            step.Write("Step");
            periodic.Write("Periodic");
            randomFactor.Write("Random Factor");
            updateRate.Write("Max Update Rate");
            nGenerations.Write("Max Generations");
            height.Write("Height");
            width.Write("Width");
            seed.Write("Seed");
        }
    }
}
