using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace
{
    public interface IMidSurface
    {
        IEnumerable<ISegment> GetData();

        void Add(ISegment segment);
    }
}
