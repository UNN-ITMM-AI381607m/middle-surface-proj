using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace MidSurface
{
    public interface IMidSurface
    {
        List<ISegment> GetData();
    }
}
