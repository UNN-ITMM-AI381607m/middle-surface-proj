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
        private System.Windows.Controls.Canvas canvas;
        
        public View(System.Windows.Controls.Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Paint(IVisibleData data)
        {
            double zoom = Math.Max(canvas.ActualWidth, canvas.ActualHeight)/data.GetMaxLinearSize();
            //TODO: Dinar: check step when parser will be updated
            double step = 1d / (Math.Round(zoom) * 100d);
            double center_X = canvas.ActualHeight/2;
            double center_Y = canvas.ActualWidth/2;

            foreach (ISegment segment in data.GetSegments())
            {           
                Point point = new Point();
                Polyline pl = new Polyline();
                
                pl.StrokeThickness = data.GetSettings().Thikness;
                pl.Stroke = data.GetSettings().Brush;
                                
                point = segment.GetCurvePoint(0);
                pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.X), center_Y - zoom * (point.Y)));

                for (double t = 0.0d+step; t < 1.0d; t += step)
                {
                    point = segment.GetCurvePoint(t);
                    pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.X), center_Y - zoom * (point.Y)));
                }

                point = segment.GetCurvePoint(1d);
                pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.X), center_Y - zoom * (point.Y)));

                canvas.Children.Add(pl);
            }
        }
    }
}
