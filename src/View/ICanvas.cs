using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace MidSurface.Component
{
    public interface ICanvas
    {
        void Draw(ISegment segment);
    }
}
