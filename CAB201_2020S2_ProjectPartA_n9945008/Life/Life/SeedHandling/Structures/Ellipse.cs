using System;

namespace Life.Structures
{
    public class Ellipse : Structure
    {
        public Ellipse(string[] data, ref Universe universe) : base(data, ref universe)
        { }

        public override void build(int[] keypoints, string state, ref Universe universe)
        {
            double x0 = (keypoints[1] + keypoints[3]) / 2;
            double y0 = (keypoints[0] + keypoints[2]) / 2;
            double dx = System.Math.Abs(keypoints[3] - keypoints[1]);
            double dy = System.Math.Abs(keypoints[2] - keypoints[0]);

            for (int row = 0; row <= universe.Settings.height; row++)
            {
                for (int column = keypoints[0]; column <= universe.Settings.width; column++)
                {
                    double numerator1 = 4 * (column - x0) * (column - x0);
                    double denominator1 = dx * dx;
                    double fraction1 = numerator1 / denominator1;
                    double numerator2 = 4 * (row - y0) * (row - y0);
                    double denominator2 = dy * dy;
                    double fraction2 = numerator2 / denominator2;
                    if ((fraction1 + fraction2) <= 1)
                    {
                        base.SetCellFromSeed(row, column, state, ref universe);
                    }
                }
            }
        }
        public override int[] stringArrayToKeypoints(string[] data)
        {
            Console.WriteLine(data);
            int[] keypoints = new int[4];
            for (int i = 0; i < 4; i++)
            {
                int.TryParse(data[i + 2], out keypoints[i]);
            }
            Console.WriteLine(keypoints);
            return keypoints;
        }
    }

}
