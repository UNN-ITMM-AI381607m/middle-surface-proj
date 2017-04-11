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
        private List<ISegment> segments;
        private VisibleDataSettings settings;
        static private Size model_size;
        private VisibleData()
        {
            SetDefaultSetting();
        }
        public VisibleData(IModel model):this()
        { 
            segments = model.GetCanvasData().ToList<ISegment>();
            model_size = model.GetSize();
        }
        public VisibleData(IModel model, VisibleDataSettings settings)
        {
            segments = model.GetCanvasData().ToList<ISegment>();
            this.settings = settings;
            model_size = model.GetSize();
        }
        public VisibleData(IMidSurface midsurface):this()
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
            settings = new VisibleDataSettings();
            settings.Thikness = 1d;
            settings.Brush = Brushes.Black;
        }

        public VisibleDataSettings GetSettings()
        {
            return settings;
        }

        public Size GetSize()
        {
            return model_size;
        }

    }
}
