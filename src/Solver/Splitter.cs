using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace Solver
{
    public class Splitter: ISplitter
    {
        public Splitter() { }

        public List<ICustomPoint> Split(IEnumerable<ISegment> segments, double accuracy)
        {
            List<ICustomPoint> custompoints = new List<ICustomPoint>();
            return custompoints;
        }
    }
}
