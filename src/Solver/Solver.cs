using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;
using Model;

namespace Solver
{
    public class Solver: ISolver
    {
        IAlgorithm algorithm;

        public Solver(IAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public IMidSurface FindSurface(IModel model)
        {
            return algorithm.Run(model);
        }
    }
}
