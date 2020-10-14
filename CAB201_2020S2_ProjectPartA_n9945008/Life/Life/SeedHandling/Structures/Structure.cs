using System;
using System.Collections.Generic;
using System.Text;

namespace Life.Structures
{
    public abstract class Structure
    {
        public Structure(string[] data, ref Universe universe){
            int[] keypoints = stringArrayToKeypoints(data);
            build(keypoints, data[0], ref universe);
        }

        public abstract void build(int[] keypoints, string state, ref Universe universe);

        public abstract int[] stringArrayToKeypoints(string[] data);

        public void SetCellFromSeed(int row, int column, string state, ref Universe universe)
        {
            // Set chosen cell
            switch (state)
            {
                case "(o)":
                    universe.SetCell(row, column, CellStates.Alive);
                    break;

                case "(x)":
                    universe.SetCell(row, column, CellStates.Dead);
                    break;

                default:
                    break;
            }
        }
    }
}
