using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;

namespace MidSurface.Solver
{
    public interface IAlgorithm
    {
        IMidSurface Run(IModel model);
    }
}
