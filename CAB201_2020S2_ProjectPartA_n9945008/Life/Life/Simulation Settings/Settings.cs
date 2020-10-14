using System;
using System.Collections.Generic;


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
        private const int MinMem = 4;
        private const int MaxMem = 512;
        private static List<int> defaultSurvival()
        {
            var defaultValues = new List<int>(){
                2,3
            };
            return defaultValues;
        }
        private static List<int> defaultBirth()
        {
            var defaultValues = new List<int>(){
                3
            };
            return defaultValues;
        }


        public BoolOption step = new BoolOption("Step", false);
        public BoolOption periodic = new BoolOption("Periodic", false);
        public BoolOption ghost = new BoolOption("Ghost", false);
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
        public IntOption generationalMemory = new IntOption("Generational Memory",
                                                                16,
                                                                MinMem,
                                                                MaxMem);
        public FileOption seed = new FileOption("Seed", "N/A");
        public FileOption outputFile = new FileOption("Output", "N/A");
        public RuleOption survival = new RuleOption("Survival Rule", defaultSurvival());
        public RuleOption birth = new RuleOption("Birth Rule", defaultBirth());
        public NeighborhoodOption neighborhoodStyle = new NeighborhoodOption("Neighborhood Style",
                                                                Neighborhood.moore,
                                                                new string[] { "moore", "vonneumann" });

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
            ghost.Write("Ghost Mode");
            randomFactor.Write("Random Factor");
            updateRate.Write("Max Update Rate");
            nGenerations.Write("Max Generations");
            generationalMemory.Write("Generation Memory");
            height.Write("Height");
            width.Write("Width");
            seed.Write("Seed");
            outputFile.Write("Output File");
            survival.Write("Survival Rule");
            birth.Write("Birth Rule");
            neighborhoodStyle.Write("Neighbourhood Style");

        }
    }
}
