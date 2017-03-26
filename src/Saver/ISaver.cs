using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;

namespace Saver
{
    public interface ISaver
    {
        void Export(IMidSurface midsurface);
    }
}
