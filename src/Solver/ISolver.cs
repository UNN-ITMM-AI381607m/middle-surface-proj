using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;
using Model;

namespace Solver
{
    public interface ISolver
    {
        IMidSurface FindSurface(IModel model);
    }
}
