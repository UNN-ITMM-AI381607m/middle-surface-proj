using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace.Primitive;
using System.Windows.Media;
namespace MidSurfaceNameSpace.View
{
    public class VisibleData:IVisibleData
    {
        List<ISegment> segments;
        VisibleDataSettings settings;

        private VisibleData()
        {
            SetDefaultSetting();
        }
        public VisibleData(IModel model):base()
        {
            segments = model.GetCanvasData().ToList<ISegment>();
        }
        public VisibleData(IModel model, VisibleDataSettings settings) 
        {
            segments = model.GetCanvasData().ToList<ISegment>();
            this.settings = settings;
        }
        public VisibleData(IMidSurface midsurface):base()
        {
            segments = midsurface.GetData().ToList<ISegment>();
        }

        public VisibleData(IMidSurface midsurface, VisibleDataSettings settings) 
        {
            segments = midsurface.GetData().ToList<ISegment>();
            this.settings = settings;
        }
        public IEnumerable<ISegment> GetSegments()
        {
            return segments;
        }
           
        private void SetDefaultSetting()
        {
            settings.Offset_X = 0d;
            settings.Offset_Y = 0d;
            settings.Thikness = 1d;
            settings.Scale = 1d;
            settings.Brush = Brushes.Black;
        }

        public VisibleDataSettings GetSettings()
        {
            return settings;
        }
    }
}
