using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public interface IJoinMSPoints
    {
        IMidSurface Join();
        //for debug
        IMidSurface Join(List<IMSPoint> points);
        IMidSurface Join(List<Point> points);
    }
}
