using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class ViewTest
    {
        [TestMethod]
        public void ViewTestWithCanvas()
        {
            //Create simple model from 2 segments which are connected to each other
            MidSurfaceNameSpace.Component.Model model = new MidSurfaceNameSpace.Component.Model();
            MidSurfaceNameSpace.Primitive.Figure figure = new MidSurfaceNameSpace.Primitive.Figure();
            MidSurfaceNameSpace.Primitive.Contour countour = new MidSurfaceNameSpace.Primitive.Contour();
            MidSurfaceNameSpace.Primitive.BezierCurve curve = new MidSurfaceNameSpace.Primitive.BezierCurve();
            System.Collections.Generic.List<System.Windows.Point> pillars = new System.Collections.Generic.List<System.Windows.Point>
            {
                new System.Windows.Point(0, 0),
                new System.Windows.Point(10, 0),
                new System.Windows.Point(10, 10),
                new System.Windows.Point(0, 10)
            };

            MidSurfaceNameSpace.Primitive.Segment segment1 = new MidSurfaceNameSpace.Primitive.Segment(curve, pillars);
            countour.Add(segment1);

            pillars.Clear();

            pillars.Add(new System.Windows.Point(0, 0));
            pillars.Add(new System.Windows.Point(-20, 0));
            pillars.Add(new System.Windows.Point(-20, 20));
            pillars.Add(new System.Windows.Point(0, 10));
            MidSurfaceNameSpace.Primitive.Segment segment2 = new MidSurfaceNameSpace.Primitive.Segment(curve, pillars);
            countour.Add(segment2);

            figure.Add(countour);
            model.Add(figure);
            
            //Create canvas with size (100,100) for drawing, set up setting, make canvas think that he is in windows, paint all stuff
            System.Windows.Controls.Canvas canvas = new System.Windows.Controls.Canvas();
            canvas.Height = 100;
            canvas.Width = 100;
            UIElement e = canvas as UIElement;
            e.RenderSize = new Size(canvas.Height,canvas.Width);

            //Prepare View and VisibleData with settings
            MidSurfaceNameSpace.Component.View view = new MidSurfaceNameSpace.Component.View(canvas);
            MidSurfaceNameSpace.View.VisibleDataSettings settings = new MidSurfaceNameSpace.View.VisibleDataSettings();
            settings.Brush = Brushes.Black;
            settings.Thikness = 1;
            MidSurfaceNameSpace.View.VisibleData visible_data = new MidSurfaceNameSpace.View.VisibleData(model, settings);
            view.Paint(visible_data);

            //Template for checking begin and end points of all segmentson canvas and in model

            //Collecting date about segments 
            System.Collections.Generic.List<MidSurfaceNameSpace.Primitive.ISegment> list_segments = model.GetCanvasData().ToList<MidSurfaceNameSpace.Primitive.ISegment>();
            //Check count of segments on canvas and segments in model
            Assert.AreEqual(canvas.Children.Count, list_segments.Count);
            
            //Get scale for current canvas and model
            double linear_size = visible_data.GetMaxLinearSize();
            double scale = Math.Max(canvas.ActualHeight,canvas.ActualWidth)/linear_size;
            
            //Check every segment
            for (int i=0; i<list_segments.Count;++i)
            {
                //Get polyline data from canvas
                Polyline pl = canvas.Children[i] as Polyline;
                MidSurfaceNameSpace.Primitive.ISegment sgmnt = list_segments[i];
                //Check that current segment has the same X coordinate on it begin and end (according to current offset and scale)
                Assert.AreEqual(pl.TranslatePoint(pl.Points[0], pl).X,sgmnt.GetCurvePoint(0).X * scale+canvas.ActualWidth/2);
                Assert.AreEqual(pl.TranslatePoint(pl.Points[pl.Points.Count-1],pl).X, sgmnt.GetCurvePoint(1).X*scale+ canvas.ActualWidth/2);
                //Check the same for Y
                Assert.AreEqual(pl.TranslatePoint(pl.Points[0],pl).Y, canvas.ActualHeight/2 - sgmnt.GetCurvePoint(0).Y*scale);
                Assert.AreEqual(pl.TranslatePoint(pl.Points[pl.Points.Count - 1],pl).Y, canvas.ActualHeight/2 - sgmnt.GetCurvePoint(1).Y*scale);
            }
         
        }
    }
}
