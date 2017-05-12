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
        Normal normal;
        Point parent;

        public MSPoint(Point mspoint, ISegment segment)
        {
            this.mspoint = mspoint;
            this.segment = segment;
            this.normal = null;
            this.parent = new Point(-1, -1);
        }

        public MSPoint(Point mspoint, ISegment segment, Normal normal, Point parent)
        {
            this.mspoint = mspoint;
            this.segment = segment;
            this.normal = normal;
            this.parent = parent;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public ISegment GetSegment()
        {
            return segment;
        }

        public Normal GetNormal()
        {
            return normal;
        }

        public Point GetParentPoint()
        {
            return parent;
        }
    }
}
