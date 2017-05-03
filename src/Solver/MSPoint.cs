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
        Point mspoint;
        ISegment segment;

        public MSPoint(Point mspoint, ISegment segment)
        {
            this.mspoint = mspoint;
            this.segment = segment;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public ISegment GetSegment()
        {
            return segment;
        }
    }
}
