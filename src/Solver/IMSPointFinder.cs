using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public interface IMSPointFinder
    {
        IMSPoint FindMSPoint(Point contourPoint, Normal normal);
    }
}
