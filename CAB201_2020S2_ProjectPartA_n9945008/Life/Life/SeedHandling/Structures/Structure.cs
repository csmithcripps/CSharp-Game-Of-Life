using System;
using System.Collections.Generic;
using System.Text;

namespace Life.Structures
{
    interface Structure
    {
        void build(string[] data, ref Universe universe);
    }
}
