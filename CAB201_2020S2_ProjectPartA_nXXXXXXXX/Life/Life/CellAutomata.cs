using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    public class CellAutomata
    {

        private int[,] cellStates;
        private int height;
        private int width;
        private bool periodic;
        public CellAutomata(int height, int width, bool periodic = false)
        {
            cellStates = new int[height, width];
            this.height = height;
            this.width = width;
            this.periodic = periodic;
        }

        public void RandomSeed(float randomFactor)
        {
            Random generator = new Random();
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    cellStates[row, column] = (generator.NextDouble() < randomFactor) ? 1 : 0;
                }
            }
        }

        public void SetCell(int row, int column, int state)
        {
            cellStates[row, column] = state;
        }

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

        public void Update()
        {
            int[,] newStates = new int[height, width];
            int livingNeighbours;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    livingNeighbours = GetLivingNeighbours(row, column);
                    if (cellStates[row, column] == 1 &&
                        (livingNeighbours == 2 || livingNeighbours == 3))
                    {
                        newStates[row, column] = 1;
                    }
                    else if (cellStates[row, column] == 0 && livingNeighbours == 3)
                    {
                        newStates[row, column] = 1;
                    }
                    else
                    {
                        newStates[row, column] = 0;
                    }
                }
            }
            cellStates = newStates;
        }

        private int GetLivingNeighbours(int row, int column)
        {
            int livingNeighbours = 0;
            int cursorRow;
            int cursorColumn;
            if (periodic)
            {
                for (int rowMod = -1; rowMod < 2; rowMod++)
                {
                    for (int columnMod = -1; columnMod < 2; columnMod++)
                    {
                        cursorRow = mod(row + rowMod, height);
                        cursorColumn = mod(column + columnMod, width);

                        if (cursorColumn == column && cursorRow == row)
                        {
                            continue;
                        }
                        livingNeighbours += cellStates[cursorRow, cursorColumn];
                    }
                }
            }
            else
            {
                for (cursorRow = row - 1; cursorRow <= row + 1; cursorRow++)
                {
                    if (cursorRow < 0 || cursorRow >= height)
                    {
                        continue;
                    }
                    for (cursorColumn = column - 1; cursorColumn <= column + 1; cursorColumn++)
                    {
                        if (cursorColumn < 0 || cursorColumn >= width ||
                            (cursorColumn == column && cursorRow == row))
                        {
                            continue;
                        }
                        livingNeighbours += cellStates[cursorRow, cursorColumn];
                    }
                }
            }
            return livingNeighbours;
        }

        private int mod(int a, int n)
        {
            if (a<0) return a+n;
            else return a%n;            
        }
    }
}