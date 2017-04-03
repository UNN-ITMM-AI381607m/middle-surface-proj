using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface;
using MidSurface.Component;
using MidSurface.Primitive;

namespace View
{
    public class VisibleData:IVisibleData
    {
        List<ISegment> segments;
        
        public VisibleData(IModel model)
        {
            segments = model.GetCanvasData().ToList<ISegment>();
        }

        public VisibleData(IMidSurface midsurface)
        {
            segments = midsurface.GetData().ToList<ISegment>();
        }

        public IEnumerable<ISegment> GetSegments()
        {
            return segments;
        }
    }
}
