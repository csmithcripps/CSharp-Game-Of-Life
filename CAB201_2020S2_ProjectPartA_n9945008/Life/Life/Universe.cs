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

        private CellStates[,] cellArray;
        private List<CellStates[,]> universeMemory;
        private Settings settings;

        /// <summary>
        /// Constructs a new instance of the Cell Automata "Universe" of a specified size
        /// All cells are initially "Dead"
        /// </summary>
        /// <param name="settings">The Simulator settings</param>
        /// 
        public Universe(Settings settings)
        {
            this.settings = settings;
            cellArray = new CellStates[settings.height, settings.width];
            universeMemory = new List<CellStates[,]>();
            universeMemory.Add(cellArray);
            universeMemory.Add(cellArray);
            universeMemory.Add(cellArray);
            universeMemory.Add(cellArray);
        }

        /// <summary>
        /// Randomises the universe based on some factor of randomness
        /// </summary>
        /// <param name="randomFactor">Percentage of cells that will be alive</param>
        ///     
        public void RandomSeed()
        {
            Random generator = new Random();

            // Run through every row and column
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {
                    // If some random number is less than the random factor, set cell to alive
                    //      else set cell to dead
                    cellArray[row, column] = (generator.NextDouble() < settings.randomFactor) ?
                        CellStates.Alive : CellStates.Dead;
                }
            }
        }

        /// <summary>
        /// Sets a cell to some Alive
        /// </summary>
        /// <param name="row">Row of the chosen cell</param>
        /// <param name="column">column of the chosen cell</param>
        /// 
        public void SetCell(int row, int column, CellStates Status)
        {
            cellArray[row, column] = Status;
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
                    if (cellArray[row, column] == CellStates.Alive)
                    {
                        state = CellState.Full;
                    }
                    else if (settings.ghost)
                    {
                        if (universeMemory[universeMemory.Count - 2][row, column] == CellStates.Alive)
                        {
                            state = CellState.Light;
                        }
                        else if (universeMemory[universeMemory.Count - 3][row, column] == CellStates.Alive)
                        {
                            state = CellState.Medium;
                        }
                        else if (universeMemory[universeMemory.Count - 4][row, column] == CellStates.Alive)
                        {
                            state = CellState.Dark;
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
            CellStates[,] newStates = new CellStates[settings.height, settings.width];
            int livingNeighbours;

            // Iterate through each cell
            for (int row = 0; row < settings.height; row++)
            {
                for (int column = 0; column < settings.width; column++)
                {
                    livingNeighbours = GetLivingNeighbours(row, column);

                    // If the cell is alive and has 2 or 3 living neighbours, stay alive
                    if ((int)cellArray[row, column] == 1 &&
                        settings.survival.value.Contains(livingNeighbours))
                    {
                        newStates[row, column] = CellStates.Alive;
                    }

                    // If the cell is dead and has exactly three living neighbours, revive
                    else if (cellArray[row, column] == 0 &&
                            settings.birth.value.Contains(livingNeighbours))
                    {
                        newStates[row, column] = CellStates.Alive;
                    }
                    // Otherwise set the cell to dead
                    else
                    {
                        newStates[row, column] = CellStates.Dead;
                    }
                }
            }

            // Update Class variable to match new state
            cellArray = newStates;
            universeMemory.Add(cellArray);
            if (universeMemory.Count > settings.generationalMemory)
            {
                universeMemory.RemoveAt(0);
            }
        }


        /// <summary>
        /// Determines the number of living neighbours of some cell
        /// </summary>
        /// <param name="row">Row of the chosen cell</param>
        /// <param name="column">Column of the chosen cell</param>
        /// <returns>The number of neighbours that are set to living</returns>
        ///   
        private int GetLivingNeighbours(int row, int column)
        {
            int livingNeighbours = 0;
            int cursorRow;
            int cursorColumn;

            for (int rowMod = -1; rowMod <= 1; rowMod++)
            {
                // If outside of range vertically and periodic conditions are false
                //      skip this row
                if (!settings.periodic && (row + rowMod > settings.height || row + rowMod < 0))
                {
                    continue;
                }

                // Calculate cursor row with wraparound boundaries
                cursorRow = (row + rowMod + settings.height) % settings.height;

                for (int columnMod = -1; columnMod <= 1; columnMod++)
                {
                    cursorColumn = (column + columnMod + settings.width) % settings.width;

                    // If outside of range horizontally and periodic conditions are false,
                    //  Or the cursor is on the cell being checked
                    //      skip this row
                    if ((cursorColumn == column && cursorRow == row) ||
                        (!settings.periodic && (column + columnMod > settings.width || column + columnMod < 0)))
                    {
                        continue;
                    }

                    // Add the integer version of the cell state to the number of living neighbours.
                    livingNeighbours += (int)cellArray[cursorRow, cursorColumn];
                }
            }

            return livingNeighbours;
        }
    }
}