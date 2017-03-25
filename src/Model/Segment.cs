using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Segment: ISegment
    {
        ICurve curve;
        List<PointF> pillar;

        public Segment(ICurve curve, List<PointF> pillar)
        {
            this.curve = curve;
            this.pillar = pillar;
        }

        public PointF GetCurvePoint(double t)
        {
            return curve.GetCurvePoint(pillar, t);
        }

        public List<PointF> GetPillar()
        {
            return pillar;
        }
    }
}
