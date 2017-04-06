using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;
using MidSurface.Component;

namespace MidSurface.Solver
{
    public class SolverData: ISolverData
    {
        List<ISegment> segments;

        public SolverData(IModel model)
        {
            segments = model.GetCanvasData().ToList<ISegment>();
        }

        public List<ISegment> GetSegments()
        {
            return segments;
        }
    }
}
