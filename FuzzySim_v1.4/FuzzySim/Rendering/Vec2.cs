using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzySim.Rendering
{
    /// <summary>
    /// Holds X and Y values (double)
    /// </summary>
    public class Vec2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
