using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class MSPoint : IMSPoint
    {
        Point mspoint;
        ISegment segment;
        double Radius;

        public MSPoint(Point mspoint, ISegment segment)
        {
            this.mspoint = mspoint;
            this.segment = segment;
        }

        public MSPoint(Point mspoint, ISegment segment, double R)
        {
            this.mspoint = mspoint;
            this.segment = segment;
            Radius = R;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public double GetRadius()
        {
            return Radius;
        }

        public ISegment GetSegment()
        {
            return segment;
        }
    }
}
