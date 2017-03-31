using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Primitive
{
    public class Segment: ISegment
    {
        ICurve curve;
        IEnumerable<IPointF> pillar;

        public Segment(ICurve curve, IEnumerable<IPointF> pillar)
        {
            this.curve = curve;
            this.pillar = pillar;
        }

        public IPointF GetCurvePoint(double t)
        {
            return curve.GetCurvePoint(pillar, t);
        }

        public IEnumerable<IPointF> GetPillar()
        {
            return pillar;
        }
    }
}
