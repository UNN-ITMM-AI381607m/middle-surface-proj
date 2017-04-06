using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public interface IMSPointFinder
    {
        List<IPointEx> FindMSPoints(List<ICustomPoint> custompoints);
        IPointEx FindBisectorPoint(ICustomPoint custompoint);
    }
}
