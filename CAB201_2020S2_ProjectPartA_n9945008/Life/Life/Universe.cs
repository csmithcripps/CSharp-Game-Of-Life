using Display;
using System;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// A class representing a "Universe" described by the laws of Conway's game of Life
    /// </summary>
    public class Universe
    {

        private CellStatus[,] cellArray;
        private UniverseMemory universeMemory;
        private CellStatus[,,] ghostMemory;
        public Settings settings;

        private NeighborhoodHandler neighborhoodHandler;

        /// <summary>
        /// Constructs a new instance of the Cell Automata "Universe" of a specified size
        /// All cells are initially "Dead"
        /// </summary>
        /// <param name="settings">The Simulator settings</param>
        /// 
        public Universe(Settings settings)
        {

            this.settings = settings;
            cellArray = new CellStatus[settings.height, settings.width];
            universeMemory = new UniverseMemory(settings.generationalMemory, ref settings);
            ghostMemory = new CellStatus[4, settings.height, settings.width];
            switch (settings.neighborhoodStyle.value)
            {
                case NeighborhoodType.moore:
                    neighborhoodHandler = new MooreNeighborhood(settings.neighborhoodStyle);
                    break;

                case NeighborhoodType.vonNeumann:
                    neighborhoodHandler = new VonNeumannNeighborhood(settings.neighborhoodStyle);
                    break;

                default:
                    neighborhoodHandler = new MooreNeighborhood(settings.neighborhoodStyle);
                    break;

            }
        }

        /// <summary>
        /// Sets a cell to some Alive
        /// </summary>
        /// <param name="row">Row of the chosen cell</param>
        /// <param name="column">column of the chosen cell</param>
        /// 
        public void SetCell(int row, int column, CellStatus Status)
        {
            cellArray[row, column] = Status;
        }

        public CellStatus GetCell(int row, int column)
        {
            return cellArray[row, column];
        }

        /// <summary>
        /// Draw current state to some grid
        /// </summary>
        /// <param name="grid">The display to draw to</param>
        /// 
        public void Draw(Grid grid)
        {
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {

                    CellState state = CellState.Blank;
                    if (cellArray[row, column] == CellStatus.Alive)
                    {
                        state = CellState.Full;
                    }
                    else if (settings.ghost)
                    {
                        if (ghostMemory[1, row, column] == CellStatus.Alive)
                        {
                            state = CellState.Dark;
                        }
                        else if (ghostMemory[2, row, column] == CellStatus.Alive)
                        {
                            state = CellState.Medium;
                        }
                        else if (ghostMemory[3, row, column] == CellStatus.Alive)
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
            CellStatus[,] newStates = new CellStatus[settings.height, settings.width];
            int livingNeighbours;

            // Iterate through each cell
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {
                    livingNeighbours = neighborhoodHandler.GetLivingNeighbors(this, row, column);

                    // If the cell is alive and has 2 or 3 living neighbours, stay alive
                    if ((int)cellArray[row, column] == 1 &&
                        settings.survival.value.Contains(livingNeighbours))
                    {
                        newStates[row, column] = CellStatus.Alive;
                    }

                    // If the cell is dead and has exactly three living neighbours, revive
                    else if (cellArray[row, column] == 0 &&
                            settings.birth.value.Contains(livingNeighbours))
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

            universeMemory.Add(cellArray);

            // Update Class variable to match new state
            cellArray = newStates;
            UpdateGhost();
        }

        private void UpdateGhost()
        {
            CellStatus[,,] tempArray = new CellStatus[4, settings.height, settings.width];
            for (int i = 0; i < 3; i++)
            {
                for (int row = 0; row < settings.height; row++)
                {
                    for (int column = 0; column < settings.width; column++)
                    {
                        tempArray[i + 1, row, column] = ghostMemory[i, row, column];

                        tempArray[0,row,column] = cellArray[row,column];
                    }
                }
            }

            ghostMemory = tempArray;
        }

        public int CheckSteadyState()
        {
            return universeMemory.Search(cellArray);
        }
    }
}