using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace MidSurface
{
    public interface IMidSurface
    {
        IEnumerable<ISegment> GetData();
    }
}
