using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace Solver
{
    public class MSPointFinder: IMSPointFinder
    {
        List<ISegment> segments;

        public MSPointFinder(List<ISegment> segments)
        {
            this.segments = segments;
        }

        public List<IPointEx> FindMSPoints(List<ICustomPoint> custompoints)
        {
            List<IPointEx> mspoints = new List<IPointEx>();

            //извольте ваш кодик-с

            return mspoints;
        }
    }
}
