using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Primitive
{
    public class BezierCurve : ICurve
    {
        public BezierCurve() { }

        public IPointF GetCurvePoint(IEnumerable<IPointF> pillar, double t)
        {
            double X_coord;
            double Y_coord;
            if (pillar.Count() == 2)
            {
                X_coord = (1 - t) * pillar.ElementAt(0).GetX() + t * pillar.ElementAt(1).GetX();
                Y_coord = (1 - t) * pillar.ElementAt(0).GetY() + t * pillar.ElementAt(1).GetY();
            }
            else if (pillar.Count() == 3)
            {
                X_coord = (1 - t) * (1 - t) * pillar.ElementAt(0).GetX() + 2 * t * (1 - t) * pillar.ElementAt(1).GetX() + t * t * pillar.ElementAt(2).GetX();
                Y_coord = (1 - t) * (1 - t) * pillar.ElementAt(0).GetY() + 2 * t * (1 - t) * pillar.ElementAt(1).GetY() + t * t * pillar.ElementAt(2).GetY();
            }
            else if (pillar.Count() == 4)
            {
                X_coord = (1 - t) * (1 - t) * (1 - t) * pillar.ElementAt(0).GetX() + 3 * t * (1 - t) * (1 - t) * pillar.ElementAt(1).GetX() + 3 * t * t * (1 - t) * pillar.ElementAt(2).GetX() + t * t * t * pillar.ElementAt(3).GetX();
                Y_coord = (1 - t) * (1 - t) * (1 - t) * pillar.ElementAt(0).GetY() + 3 * t * (1 - t) * (1 - t) * pillar.ElementAt(1).GetY() + 3 * t * t * (1 - t) * pillar.ElementAt(2).GetY() + t * t * t * pillar.ElementAt(3).GetY();
            }
            else
                return new PointF();
            return new PointF(X_coord, Y_coord);
        }
    }
}
