using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class BezierCurve : ICurve
    {
        public BezierCurve() { }

        public PointF GetCurvePoint(List<PointF> pillar, double t)
        {
            double X_coord;
            double Y_coord;
            if (pillar.Count() == 2)
            {
                X_coord = (1 - t) * pillar[0].GetX() + t * pillar[1].GetX();
                Y_coord = (1 - t) * pillar[0].GetY() + t * pillar[1].GetY();
            }
            if (pillar.Count() == 2)
            {
                X_coord = (1 - t) * (1 - t) * pillar[0].GetX() + 2 * t * (1 - t) * pillar[1].GetX() + t * t * pillar[2].GetX();
                Y_coord = (1 - t) * (1 - t) * pillar[0].GetY() + 2 * t * (1 - t) * pillar[1].GetY() + t * t * pillar[2].GetY();
            }
            if (pillar.Count() == 3)
            {
                X_coord = (1 - t) * (1 - t) * (1 - t) * pillar[0].GetX() + 3 * t * (1 - t) * (1 - t) * pillar[1].GetX() + 3 * t * t * (1 - t) * pillar[2].GetX() + t * t * t * pillar[3].GetX();
                Y_coord = (1 - t) * (1 - t) * (1 - t) * pillar[0].GetY() + 3 * t * (1 - t) * (1 - t) * pillar[1].GetY() + 3 * t * t * (1 - t) * pillar[2].GetY() + t * t * t * pillar[3].GetY();
            }
            else
                return new PointF();
            return new PointF(X_coord, Y_coord);
        }
    }
}
