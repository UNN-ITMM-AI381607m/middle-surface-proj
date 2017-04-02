using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;
using MidSurface.Primitive;

namespace Solver
{
    public interface ISolverData
    {
        List<ISegment> GetSegments();
    }
}
