using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public class Segment : ISegment
    {
        ICurve curve;
        List<Point> pillar;

        public Segment(ICurve curve, List<Point> pillar)
        {
            this.curve = curve;
            this.pillar = pillar;
        }

        public Point GetCurvePoint(double t)
        {
            return curve.GetCurvePoint(pillar, t);
        }

        public List<Point> GetPillar()
        {
            return pillar;
        }

        public Normal GetNormal(double t)
        {
            var point1 = curve.GetCurvePoint(pillar, t);
            var point2 = curve.GetCurvePoint(pillar, t + Normal.T_OFFSET);
            var dx = point1.X - point2.X;
            var dy = point2.Y - point1.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            return new Normal(this, t, dy / length, dx / length);
        }
    }
}
