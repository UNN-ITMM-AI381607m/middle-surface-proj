using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public class Segment: ISegment
    {
        ICurve curve;
        IEnumerable<Point> pillar;

        public Segment(ICurve curve, IEnumerable<Point> pillar)
        {
            this.curve = curve;
            this.pillar = pillar;
        }

        public Point GetCurvePoint(double t)
        {
            return curve.GetCurvePoint(pillar, t);
        }

        public IEnumerable<Point> GetPillar()
        {
            return pillar;
        }
    }
}
