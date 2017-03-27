using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace MidSurface
{
    public class MidSurface : IMidSurface
    {
        List<ISegment> segments;

        public MidSurface()
        {
            segments = new List<ISegment>();
        }

        public void Add(ISegment segment)
        {
            segments.Add(segment);
        }

        public IEnumerable<ISegment> GetData()
        {
            return segments;
        }
    }
}
