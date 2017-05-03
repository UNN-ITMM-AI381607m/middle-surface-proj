using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public interface ISegment
    {
        Point GetCurvePoint(double t);
        List<Point> GetPillar();
        Normal GetNormal(double t);
    }
}
