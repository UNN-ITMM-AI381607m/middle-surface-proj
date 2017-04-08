using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class MSPoint:IMSPoint
    {
        ICustomLine line;
        Point mspoint;

        public MSPoint(Point mspoint, ICustomLine line)
        {
            this.mspoint = mspoint;
            this.line = line;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public ICustomLine GetLine()
        {
            return line;
        }
    }
}
