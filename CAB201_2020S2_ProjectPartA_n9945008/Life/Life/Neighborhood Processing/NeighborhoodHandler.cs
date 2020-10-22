namespace Life.Neighborhood
{
    /// <summary>
    /// A superclass for classes that handle Getting the number of 
    /// living neighbours to a cell
    /// </summary>
    public abstract class NeighborhoodHandler
    {
        public int order;
        public bool centerCount;


        public NeighborhoodHandler(NeighborhoodOption Options)
        {
            order = Options.order;
            centerCount = Options.centerCount;
        }

        /// <summary>
        ///     Returns the number of neighbours of a given cell in some universe
        /// </summary>
        /// <param name="universe">
        ///     The universe in question
        /// <param name="row">
        ///     The row of the cell in question
        /// </param>
        /// <param name="column">
        ///     The column of the cell in question
        /// </param>
        public abstract int GetLivingNeighbors(Universe universe, int row,
                                                int column);
    }
}
