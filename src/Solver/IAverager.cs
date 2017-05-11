using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    interface IAverager
    {
        List<Point> GetAveragePath(List<List<Point>> paths);
    }
}
