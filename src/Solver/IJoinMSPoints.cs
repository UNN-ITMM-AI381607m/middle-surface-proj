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
        //for debug
        IMidSurface Join(List<IMSPoint> points);
        IMidSurface Join(List<Point> points);
        IMidSurface Join(Graph graph);
    }
}
