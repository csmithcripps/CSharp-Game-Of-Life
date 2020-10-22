using Display;

namespace Life
{
    /// <summary>
    /// A class representing a "Universe" described by the laws of Conway's game of Life
    /// </summary>
    public class Universe
    {
        private const int ghostGenerations = 4;
        private CellStatus[,] cellArray;
        private readonly UniverseMemory ghostMemory;
        private readonly Neighborhood.NeighborhoodHandler neighborhoodHandler;


        public Settings Settings { get; }

        /// <summary>
        /// Constructs a new instance of the Cell Automata "Universe" of a specified size
        /// All cells are initially "Dead"
        /// </summary>
        /// <param name="settings">The Simulator settings</param>
        /// 
        public Universe(Settings settings)
        {

            Settings = settings;
            cellArray = new CellStatus[settings.height, settings.width];

            // Set up ghost mode
            if (settings.ghost)
            {
                ghostMemory = new UniverseMemory(ghostGenerations, settings);
                ghostMemory.Add(cellArray);
            }

            // Choose Neighborhood handler
            neighborhoodHandler = settings.neighborhoodStyle.value switch
            {
                NeighborhoodType.moore => new Neighborhood.MooreNeighborhood(settings.neighborhoodStyle),
                NeighborhoodType.vonNeumann => new Neighborhood.VonNeumannNeighborhood(settings.neighborhoodStyle),
                _ => new Neighborhood.MooreNeighborhood(settings.neighborhoodStyle),
            };
        }

        /// <summary>
        /// Allows Indexing of this class
        /// </summary>
        /// <param name="row">Row of the chosen cell</param>
        /// <param name="column">column of the chosen cell</param>
        /// 
        public CellStatus this[int row, int column]
        {
            get { return cellArray[row, column];  }
            set { cellArray[row, column] = value; }
        }


        /// <summary>
        /// Draw current state to some grid
        /// </summary>
        /// <param name="grid">The display to draw to</param>
        /// 
        public void Draw(Grid grid)
        {
            // Draw cell by cell
            for (int row = 0; row < Settings.height; row++)
            {
                for (int column = 0; column < Settings.width; column++)
                {
                    // Select Cell State
                    CellState state = CellState.Blank;
                    if (cellArray[row, column] == CellStatus.Alive)
                    {
                        state = CellState.Full;
                    }
                    // Check ghost cells only if ghost mode is active
                    else if (Settings.ghost)
                    {
                        if (ghostMemory[1][row, column] == CellStatus.Alive)
                        {
                            state = CellState.Dark;
                        }
                        else if (ghostMemory[2][row, column] == CellStatus.Alive)
                        {
                            state = CellState.Medium;
                        }
                        else if (ghostMemory[3][row, column] == CellStatus.Alive)
                        {
                            state = CellState.Light;
                        }

                    }
                    grid.UpdateCell(row, column, state);
                }
            }
        }

        /// <summary>
        /// Update the universe one generation
        /// </summary>
        /// 
        public void Update()
        {
            // Set up the next generation array
            CellStatus[,] newStates = new CellStatus[Settings.height, Settings.width];
            int livingNeighbours;

            // Iterate through each cell
            for (int row = 0; row < Settings.height; row++)
            {
                for (int column = 0; column < Settings.width; column++)
                {
                    // Check Neighbours using chosen neighborhood handler
                    livingNeighbours = neighborhoodHandler.GetLivingNeighbors(this, row, column);

                    // If the cell is alive and matches the survival rule, stay alive
                    if ((int)cellArray[row, column] == 1 &&
                        Settings.survival.value.Contains(livingNeighbours))
                    {
                        newStates[row, column] = CellStatus.Alive;
                    }

                    // If the cell is dead and matches the birth rule, revive
                    else if (cellArray[row, column] == 0 &&
                            Settings.birth.value.Contains(livingNeighbours))
                    {
                        newStates[row, column] = CellStatus.Alive;
                    }
                    // Otherwise set the cell to dead
                    else
                    {
                        newStates[row, column] = CellStatus.Dead;
                    }
                }
            }

            // Update cellArray to match new state
            cellArray = newStates;
            ghostMemory.Add(cellArray);
        }

        /// <summary>
        /// This class can be implicitly converted to a 2D CellStatus array
        /// </summary>
        /// 
        public static implicit operator CellStatus[,](Universe I) => I.cellArray;
    }
}