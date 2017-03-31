using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;

namespace MidSurface.Solver
{
    public class Algorithm : IAlgorithm
    {
        public IMidSurface Run(IModel model)
        {
            IMidSurface midsurface = new MidSurface();
            return midsurface;
        }
    }
}
