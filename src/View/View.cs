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

#if DEBUG
        double zoom=1;
        System.Windows.Point center;
        public void ChangeZoom(double zoom)
        {
            this.zoom += zoom;
        }
        public void ChangeCenter(System.Windows.Point  center)
        {
            this.center = center;
        }
#endif
        private System.Windows.Controls.Canvas canvas;
        private TransformData transform_data;
        bool addIndices;
        int indexFontSize;

        public View(System.Windows.Controls.Canvas canvas)
        {
            this.canvas = canvas;
            addIndices = false;
            indexFontSize = 10;            
        }

        public void Paint(IVisibleData data)
        {
            transform_data.model_center_X = (data.GetSize().Xmax + data.GetSize().Xmin) / 2;
            transform_data.model_center_Y = (data.GetSize().Ymax + data.GetSize().Ymin) / 2;
            transform_data.center_X = canvas.ActualWidth / 2;
            transform_data.center_Y = canvas.ActualHeight/ 2;
            // scale will be mult to 0.98 in purpose of creating borders
            transform_data.scale = 0.9d * ( Math.Min(canvas.ActualWidth/ Math.Abs(data.GetSize().Xmax - data.GetSize().Xmin), canvas.ActualHeight/ Math.Abs(data.GetSize().Ymax - data.GetSize().Ymin)));
#if DEBUG
            transform_data.scale = this.zoom;
            
            transform_data.center_X = center.X;
            transform_data.center_Y = center.Y;
            
#endif
            double step = 1d / (100d);
            int index = 0;
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

               var posPoint = TransfromFill(segment.GetCurvePoint(0.0d));
               pl.Points.Add(TransfromFill(point));
       
                if (addIndices)
                    AddIndex(TransfromFill(point), index++);
                
                canvas.Children.Add(pl);
            }
        }

        private Point TransfromFill(Point p)
        {
            return new Point(transform_data.scale*(p.X - transform_data.model_center_X) +transform_data.center_X, transform_data.center_Y - transform_data.scale * (p.Y - transform_data.model_center_Y));
        }

        private void AddIndex(Point posP, int index)
        {
            System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock();
            textBlock.Text = index.ToString();
            
            textBlock.Foreground = Brushes.DarkGreen;
            textBlock.FontSize = indexFontSize;
            
            textBlock.SetValue(System.Windows.Controls.Canvas.LeftProperty, posP.X);
            textBlock.SetValue(System.Windows.Controls.Canvas.TopProperty, posP.Y);
            canvas.Children.Add(textBlock);
        }

        public void EnableIndices(bool enabled)
        {
            addIndices = enabled;

        }
        public void SetIndexFontSize(int size)
        {
            indexFontSize = size;
        }

    }
}
