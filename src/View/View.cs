using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.View;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MidSurfaceNameSpace.Component
{
    public class View: IView
    {
        struct TransformData
        {
            public double model_center_X;
            public double model_center_Y;
            public double center_X;
            public double center_Y;
            public double scale;
        }

        private System.Windows.Controls.Canvas canvas;
        private TransformData transform_data;
        public View(System.Windows.Controls.Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Paint(IVisibleData data)
        {
            transform_data.model_center_X = (data.GetSize().Xmax - data.GetSize().Xmin) / 2;
            transform_data.model_center_Y = (data.GetSize().Ymax - data.GetSize().Ymin) / 2;
            transform_data.center_X = canvas.ActualWidth / 2;
            transform_data.center_Y = canvas.ActualHeight/ 2;
            // scale will be mult to 0.9 in purpose of creating borders
            transform_data.scale = 0.9d * ( Math.Min(canvas.ActualWidth, canvas.ActualHeight) / Math.Max(Math.Abs(data.GetSize().Xmax-data.GetSize().Xmin), Math.Abs(data.GetSize().Ymax- data.GetSize().Ymin)));
            double step = 1d / (Math.Round(transform_data.scale) * 100d);
            foreach (ISegment segment in data.GetSegments())
            {           
                Point point = new Point();
                Polyline pl = new Polyline();
                
                pl.StrokeThickness = data.GetSettings().Thikness;
                pl.Stroke = data.GetSettings().Brush;
                
                for (double t = 0.0d; t < 1.0d; t += step)
                {
                    point = segment.GetCurvePoint(t);
                    pl.Points.Add(TransfromFill(point));
                }

                //The last point shoud be added
                point = segment.GetCurvePoint(1d);
                pl.Points.Add(TransfromFill(point));

                canvas.Children.Add(pl);
            }
        }

        private Point TransfromFill(Point p)
        {
            return new Point(transform_data.scale*(p.X - transform_data.model_center_X) +transform_data.center_X, transform_data.center_Y - transform_data.scale * (p.Y - transform_data.model_center_Y));
        }
    }
}
