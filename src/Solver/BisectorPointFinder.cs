using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace Solver
{
    public partial class MSPointFinder : IMSPointFinder
    {
        public IPointEx FindBisectorPoint(ICustomPoint custompoint)
        {
            var point = segments[custompoint.GetN()].GetCurvePoint(custompoint.GetT());
            var newPoint = new PointF(point.GetX() * custompoint.GetAlpha(), point.GetY());

            throw new NotImplementedException();
        }
    }
}
