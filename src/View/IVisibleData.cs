using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Component;
using MidSurface.Primitive;
using MidSurface;
using System.Windows.Media;

namespace View
{
    public struct VisibleDataSettings
    {
        public double Offset_X { get; set; }
        public double Offset_Y { get; set; }
        public double Thikness { get; set; }
        public double Scale { get; set; }
        public Brush Brush { get; set; }
    }

    public interface IVisibleData
        {
            IEnumerable<ISegment> GetSegments();
            VisibleDataSettings  GetSettings();
        }

}
