using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.Solver
{
    public partial class MSPointFinder : IMSPointFinder
    {
        public IMSPoint FindBisectorPoint(ICustomLine line1, ICustomLine line2)
        {
            var bisector = line1.GetRightNormal() + line2.GetRightNormal();
            throw new NotImplementedException();
        }
    }
}
