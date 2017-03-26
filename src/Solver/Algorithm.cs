using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;
using Model;

namespace Solver
{
    public class Algorithm: IAlgorithm
    {
        public IMidSurface Run(IModel model)
        {
            IMidSurface midsurface = new MidSurface.MidSurface();
            return midsurface;
        }
    }
}
