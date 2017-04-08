using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public class BezierCurve : ICurve
    {
        public BezierCurve() { }

        public Point GetCurvePoint(IEnumerable<Point> pillar, double t)
        {
            double X_coord;
            double Y_coord;
            if (pillar.Count() == 2)
            {
                X_coord = (1 - t) * pillar.ElementAt(0).X + t * pillar.ElementAt(1).X;
                Y_coord = (1 - t) * pillar.ElementAt(0).Y + t * pillar.ElementAt(1).Y;
            }
            else if (pillar.Count() == 3)
            {
                X_coord = (1 - t) * (1 - t) * pillar.ElementAt(0).X + 2 * t * (1 - t) * pillar.ElementAt(1).X + t * t * pillar.ElementAt(2).X;
                Y_coord = (1 - t) * (1 - t) * pillar.ElementAt(0).Y + 2 * t * (1 - t) * pillar.ElementAt(1).Y + t * t * pillar.ElementAt(2).Y;
            }
            else if (pillar.Count() == 4)
            {
                X_coord = (1 - t) * (1 - t) * (1 - t) * pillar.ElementAt(0).X + 3 * t * (1 - t) * (1 - t) * pillar.ElementAt(1).X + 3 * t * t * (1 - t) * pillar.ElementAt(2).X + t * t * t * pillar.ElementAt(3).X;
                Y_coord = (1 - t) * (1 - t) * (1 - t) * pillar.ElementAt(0).Y + 3 * t * (1 - t) * (1 - t) * pillar.ElementAt(1).Y + 3 * t * t * (1 - t) * pillar.ElementAt(2).Y + t * t * t * pillar.ElementAt(3).Y;
            }
            else
                return new Point();
            return new Point(X_coord, Y_coord);
        }
    }
}
