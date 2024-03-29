﻿using System;

namespace Life.Structures
{
    public class Cell : Structure
    {
        public Cell(string[] data, ref Universe universe) : base(data, ref universe)
        { }
        public override void build(int[] keypoints, string state, ref Universe universe)
        {
            base.SetCellFromSeed(keypoints[0], keypoints[1], state, ref universe);
        }
        public override int[] stringArrayToKeypoints(string[] data)
        {
            int[] keypoints = new int[2];
            int.TryParse(data[2], out keypoints[0]);
            int.TryParse(data[3], out keypoints[1]);
            return keypoints;
        }
    }

}
