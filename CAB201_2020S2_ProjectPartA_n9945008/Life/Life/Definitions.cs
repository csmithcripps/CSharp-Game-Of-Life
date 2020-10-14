using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Display;

namespace Life
{
    public enum CellStates
    {
        Alive = 1,
        Dead = 0
    };

    public enum Neighborhood
    {
        moore,
        vonNeumann
    };
}