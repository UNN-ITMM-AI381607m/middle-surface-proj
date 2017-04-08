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
        ICustomLine parents;
        Point mspoint;

        public MSPoint(Point mspoint, ICustomLine parents)
        {
            this.mspoint = mspoint;
            this.parents = parents;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public ICustomLine GetLine()
        {
            return parents;
        }
    }
}
