using System;

namespace Life.Neighborhood
{
    /// <summary>
    /// A class that handles getting living neighbours using a 
    /// VonNeumann Neighbourhood
    /// </summary>
    public class VonNeumannNeighborhood : NeighborhoodHandler
    {
        public VonNeumannNeighborhood(NeighborhoodOption option) : base(option)
        {
        }

        /// <summary>
        ///     Returns the number of neighbours of a given cell in some universe
        ///     Using VonNeumann Neighborhood
        /// </summary>
        /// <param name="universe">
        ///     The universe in question
        /// <param name="row">
        ///     The row of the cell in question
        /// </param>
        /// <param name="column">
        ///     The column of the cell in question
        /// </param>
        public override int GetLivingNeighbors(Universe universe, int row,
                                                int column)
        {
            int livingNeighbours = 0;
            int cursorRow;
            int cursorColumn;
            bool periodic = universe.Settings.periodic;
            int height = universe.Settings.height;
            int width = universe.Settings.width;
            

            for (int rowMod = -order; rowMod <= order; rowMod++)
            {
                // If outside of range vertically and universe.settings.periodic conditions are false
                //      skip this row
                if (!periodic &&
                    (row + rowMod > height || row + rowMod < 0))
                {
                    continue;
                }

                // Calculate cursor row with wraparound boundaries
                cursorRow = (row + rowMod + height) % height;

                for (int columnMod = -order; columnMod <= order; columnMod++)
                {
                    // Check if not in neighborhood
                    if(Math.Abs(rowMod) + Math.Abs(columnMod) > order)
                    {
                        continue;
                    }
                    
                    cursorColumn = (column + columnMod + width)
                                        % width;

                    // If outside of range horizontally and periodic conditions are false,
                    //  Or the cursor is on the cell being checked
                    //      skip this row
                    if ((!periodic &&
                        (column + columnMod > width || column + columnMod < 0)))
                    {
                        continue;
                    }

                    if (!centerCount &&
                        (cursorColumn == column && cursorRow == row))
                    {
                        continue;
                    }

                    

                    // Add the integer version of the cell state to the number of living neighbours.
                    livingNeighbours += (int)universe[cursorRow, cursorColumn];
                }
            }

            return livingNeighbours;
        }
    }
}
