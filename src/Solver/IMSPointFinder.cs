using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.Solver
{
    public interface IMSPointFinder
    {
        List<IMSPoint> FindMSPoints(List<ICustomLine> customline);
        IMSPoint FindBisectorPoint(ICustomLine customline);
    }
}
