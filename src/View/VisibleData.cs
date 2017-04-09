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
        private double max_linear_size;
        private VisibleData()
        {
            SetDefaultSetting();
        }
        //TODO: Dinar:  change next method when IModel will be updated
        private void CalculateMaxLinearSize()
        {
            double Xmax = 0, Xmin = 0, Ymax = 0, Ymin = 0;

            for (int i = 0; i < segments.Count; i++)
            {
                for (double t = 0; t < 1; t += 0.1)
                {
                    double X = segments[i].GetCurvePoint(t).X;
                    double Y = segments[i].GetCurvePoint(t).Y;
                    if (X < Xmin)
                        Xmin = X;
                    else if (X > Xmax)
                        Xmax = X;

                    if (Y < Ymin)
                        Ymin = Y;
                    else if (Y > Ymax)
                        Ymax = Y;
                }
            }

            max_linear_size = Math.Max(Math.Abs(Xmax - Xmin), Math.Abs(Ymax - Ymin));

        }
        public VisibleData(IModel model):this()
        { 
            segments = model.GetCanvasData().ToList<ISegment>();
            this.CalculateMaxLinearSize();
        }
        public VisibleData(IModel model, VisibleDataSettings settings)
        {
            segments = model.GetCanvasData().ToList<ISegment>();
            this.CalculateMaxLinearSize();
            this.settings = settings;
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

        public double GetMaxLinearSize()
        {
            return max_linear_size;
        }
    }
}
