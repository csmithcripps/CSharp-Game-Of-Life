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

            for (int y = keypoints[0]; y <= keypoints[2]; y++)
            {
                for (int x = keypoints[0]; x <= keypoints[2]; x++)
                {
                    double numerator1 = 4 * (x - x0) * (x - x0);
                    double denominator1 = dx * dx;
                    double fraction1 = numerator1 / denominator1;
                    double numerator2 = 4 * (y - y0) * (y - y0);
                    double denominator2 = dy * dy;
                    double fraction2 = numerator2 / denominator2;
                    if (fraction1 + fraction2 <= 1)
                    {
                        base.SetCellFromSeed(y, x, state, ref universe);
                    }
                }
            }
        }
        public override int[] stringArrayToKeypoints(string[] data)
        {
            Console.WriteLine(string.Join(',', data));
            int[] keypoints = new int[4];
            for (int i = 0; i < 4; i++)
            {

                int.TryParse(data[i + 2], out keypoints[i]);
            }
            return keypoints;
        }
    }

}
