namespace Life
{
    public class NeighborhoodHandler
    {
        public int order;
        public bool centerCount;
        public NeighborhoodHandler(NeighborhoodOption Options)
        {
            this.order = Options.order;
            this.centerCount = Options.centerCount;
        }

        public virtual int GetLivingNeighbors(Universe universe, int row, 
                                                int column)
        {
            throw new System.NotImplementedException();
        }
    }
}
