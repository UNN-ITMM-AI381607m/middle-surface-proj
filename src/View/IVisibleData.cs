using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace;
using System.Windows.Media;

namespace MidSurfaceNameSpace.View
{
    public struct VisibleDataSettings
    {
        public double Thikness { get; set; }
        public Brush Brush { get; set; }
#if DEBUG
        public double Zoom { get; set; }
        public System.Windows.Point Center { get; set; }
#endif
    }

    public interface IVisibleData
        {
            IEnumerable<ISegment> GetSegments();
            VisibleDataSettings  GetSettings();
            Size GetSize();
        }

}
