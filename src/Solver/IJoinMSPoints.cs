using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace;

namespace MidSurfaceNameSpace.Solver
{
    public interface IJoinMSPoints
    {
        IMidSurface Join(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy);
    }
}
