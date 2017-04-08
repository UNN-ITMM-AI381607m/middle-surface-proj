using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Solver
{
    public class Solver: ISolver
    {
        IAlgorithm algorithm;

        public Solver(IAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public IMidSurface FindSurface(ISolverData solverdata)
        {
            return algorithm.Run(solverdata);
        }
    }
}
