using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public interface ICurve
    {
        Point GetCurvePoint(IEnumerable<Point> pillar, double t); 
    }
}
