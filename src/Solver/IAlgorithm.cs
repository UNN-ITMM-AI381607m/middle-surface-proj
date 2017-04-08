using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Solver
{
    public interface IAlgorithm
    {
        IMidSurface Run(ISolverData solverdata);
    }
}
