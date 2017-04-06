using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;

namespace Solver
{
    public interface IJoinMSPoints
    {
        IMidSurface Join(IMSPointFinder mspointfinder, List<IPointEx> mspoints, double accuracy);
    }
}
