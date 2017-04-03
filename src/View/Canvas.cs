using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MidSurface.Component
{
    public class Canvas : ICanvas
    {
        private System.Windows.Controls.Canvas canvas;
        public Canvas(System.Windows.Controls.Canvas canvas)
        {
            this.canvas = canvas;
        }
        public void Draw(ISegment segment)
        {
            //TODO: lift up settings for that stuff
            double step = 0.001d;
            double zoom = 5.0d;

            IPointF point = new PointF();
            Polyline pl = new Polyline();

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Colors.Black;
            pl.StrokeThickness = 1;
            pl.Stroke = Brushes.Black;

            //TODO: Avoid that trick 
            double center_X = canvas.ActualWidth / 2;
            double center_Y = canvas.ActualHeight / 2;

            for (double t = 0.0d; t <= 1.0d; t += step)
            {
                point = segment.GetCurvePoint(t);
                pl.Points.Add(new System.Windows.Point(center_X + zoom * (point.GetX()), center_Y - zoom * (point.GetY())));
            }
            canvas.Children.Add(pl);
        }
    }
}
