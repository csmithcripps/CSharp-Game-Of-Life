using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    enum CellStates
    {
        Alive = 1,
        Dead = 0
    };
    /// <summary>
    /// A class representing a "Universe" described by the laws of Conway's game of Life
    /// </summary>
    public class Universe
    {

        private CellStates[,] cellStates;
        private int height;
        private int width;
        private bool periodic;

        /// <summary>
        /// Constructs a new instance of the Cell Automata "Universe" of a specified size
        /// All cells are initially "Dead"
        /// </summary>
        /// <param name="height">Height in rows</param>
        /// <param name="width">Width in columns</param>
        /// <param name="periodic">Whether or not periodic boundaries are enabled</param>
        /// 
        public Universe(int height, int width, bool periodic = false)
        {
            cellStates = new CellStates[height, width];
            this.height = height;
            this.width = width;
            this.periodic = periodic;
        }

        public Universe(Settings settings)
        {
            this.height = settings.height;
            this.width = settings.width;
            this.periodic = settings.periodic;
            cellStates = new CellStates[this.height, this.width];
        }

        /// <summary>
        /// Randomises the universe based on some factor of randomness
        /// </summary>
        /// <param name="randomFactor">Percentage of cells that will be alive</param>
        ///     
        public void RandomSeed(float randomFactor)
        {
            Random generator = new Random();

            // Run through every row and column
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    // If some random number is less than the random factor, set cell to alive
                    //      else set cell to dead
                    cellStates[row, column] = (generator.NextDouble() < randomFactor) ? 
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
        public void SetCellAlive(int row, int column)
        {
            cellStates[row, column] = CellStates.Alive;
        }

        /// <summary>
        /// Draw current state to some grid
        /// </summary>
        /// <param name="grid">The display to draw to</param>
        /// 
        public void Draw(Grid grid)
        {
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    grid.UpdateCell(row, column, (CellState)cellStates[row, column]);
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
            CellStates[,] newStates = new CellStates[height, width];
            int livingNeighbours;

            // Iterate through each cell
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                { 
                    livingNeighbours = GetLivingNeighbours(row, column);

                    // If the cell is alive and has 2 or 3 living neighbours, stay alive
                    if ((int)cellStates[row, column] == 1 &&
                        (livingNeighbours == 2 || livingNeighbours == 3))
                    {
                        newStates[row, column] = CellStates.Alive;
                    }

                    // If the cell is dead and has exactly three living neighbours, revive
                    else if (cellStates[row, column] == 0 && livingNeighbours == 3)
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
            cellStates = newStates;
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
                if (!periodic && (row + rowMod > height || row + rowMod < 0))
                {
                    continue;
                }

                // Calculate cursor row with wraparound boundaries
                cursorRow = (row + rowMod + height) % height;

                for (int columnMod = -1; columnMod <= 1; columnMod++)
                {
                    cursorColumn = (column + columnMod + width) % width;

                    // If outside of range horizontally and periodic conditions are false,
                    //  Or the cursor is on the cell being checked
                    //      skip this row
                    if ((cursorColumn == column && cursorRow == row) ||
                        (!periodic && (column + columnMod > width || column + columnMod < 0)))
                    {
                        continue;
                    }

                    // Add the integer version of the cell state to the number of living neighbours.
                    livingNeighbours += (int)cellStates[cursorRow, cursorColumn];
                }
            }

            return livingNeighbours;
        }
    }
}