using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.Primitive
{
    public interface IContour: IEnumerable<ISegment>
    {
        void Add(ISegment segment);
        IEnumerable<ISegment> GetSegments();
    }
}
