namespace Life
{
    public class MooreNeighborhood : NeighborhoodHandler
    {
        public MooreNeighborhood(NeighborhoodOption options) : base(options)
        {
        }

        public override int GetLivingNeighbors(Universe universe, int row,
                                                int column)
        {
            int livingNeighbours = 0;
            int cursorRow;
            int cursorColumn;
            bool periodic = universe.settings.periodic;
            int height = universe.settings.height;
            int width = universe.settings.width;
            

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
                    livingNeighbours += (int)(universe.GetCell(cursorRow, cursorColumn));
                }
            }

            return livingNeighbours;
        }
    }
}
