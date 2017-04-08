using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.IO
{
    public interface ISaver
    {
        void Export(IMidSurface midsurface);
    }
}
