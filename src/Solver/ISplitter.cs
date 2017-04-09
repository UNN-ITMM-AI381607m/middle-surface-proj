using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public interface ISplitter
    {
        List<ICustomLine> Split(IEnumerable<IContour> contours, double accuracy); 
    }
}
