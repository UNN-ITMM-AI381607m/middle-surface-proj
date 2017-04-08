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
            foreach (ISegment segment in data.GetSegments())
            {
                
                double zoom = data.GetSettings().Scale;
                double step = 1d/(zoom*100d);
                IPointF point = new PointF();
                Polyline pl = new Polyline();
                pl.StrokeThickness = data.GetSettings().Thikness;
                //TODO: check brushes
                pl.Stroke = data.GetSettings().Brush;
                double center_X = data.GetSettings().Offset_X;
                double center_Y = data.GetSettings().Offset_Y;

                point = segment.GetCurvePoint(0);
                pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.GetX()), center_Y - zoom * (point.GetY())));

                for (double t = 0.0d+step; t <= 1.0d; t += step)
                {
                    point = segment.GetCurvePoint(t);
                    pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.GetX()), center_Y - zoom * (point.GetY())));
                }
                point = segment.GetCurvePoint(1d);
                pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.GetX()), center_Y - zoom * (point.GetY())));

                canvas.Children.Add(pl);
            }
               //DrawSegment(segment);
        }
    }
}
