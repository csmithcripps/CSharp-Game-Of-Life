using System;

namespace Life.Structures
{
    public class Rectangle : Structure
    {
        public Rectangle(string[] data, ref Universe universe) : base(data, ref universe)
        { }
        public override void build(int[] keypoints, string state, ref Universe universe)
        {
            for (int row = keypoints[0]; row <= keypoints[2]; row++)
            {
                for (int column = keypoints[0]; column <= keypoints[2]; column++)
                {
                    base.SetCellFromSeed(row, column, state, ref universe);
                }
            }
        }
        public override int[] stringArrayToKeypoints(string[] data)
        {
            int[] keypoints = new int[4];
            for (int i = 0; i < 4; i++)
            {
                int.TryParse(data[i + 2], out keypoints[i]);
            }
            return keypoints;
        }
    }

}
