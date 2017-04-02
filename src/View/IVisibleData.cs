using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;
using MidSurface.Primitive;
using MidSurface;

namespace View
{
    public interface IVisibleData
        {
            IEnumerable<ISegment> GetSegments();
        }

}
