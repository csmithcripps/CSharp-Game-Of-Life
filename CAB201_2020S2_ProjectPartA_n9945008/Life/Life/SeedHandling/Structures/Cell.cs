using System;
using System.Collections.Generic;
using System.Text;

namespace Life.Structures
{
    interface Cell
    {
        static void build(string[] data, ref Universe universe)
        {
            Console.WriteLine(data[0] + data[1] + data[2] + " , " + data[3]);
            int row, column;
            int.TryParse(data[2], out row);
            Console.WriteLine(row);
            int.TryParse(data[3], out column);
            Console.WriteLine(column);

            // Set chosen cell to alive
            switch (data[0])
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
