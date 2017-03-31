using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Primitive
{
    public class Contour: IContour
    {
        List<ISegment> segments;

        public Contour()
        {
            segments = new List<ISegment>();
        }

        public void Add(ISegment segment)
        {
            segments.Add(segment);
        }

        public IEnumerator<ISegment> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISegment> GetSegments()
        {
            return segments;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
