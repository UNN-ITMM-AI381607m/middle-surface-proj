using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;

namespace MidSurface.Solver
{
    public interface ISolver
    {
        IMidSurface FindSurface(IModel model);
    }
}
