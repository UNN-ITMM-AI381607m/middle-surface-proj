using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;
using MidSurface;

namespace Solver
{
    public class JoinMSPoints: IJoinMSPoints
    {
        public JoinMSPoints() { }

        public IMidSurface Join(IMSPointFinder mspointfinder, List<IPointEx> mspoints, double accuracy)
        {
            IMidSurface midsurface = new MidSurface.MidSurface();
            return midsurface;
        }
    }
}
