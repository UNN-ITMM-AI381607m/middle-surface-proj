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
        public MidSurfaceNameSpace.Component.Model CreateSomeModel(System.Collections.Generic.List<System.Windows.Point> list_of_points)
        {
            //Create simple model from ALL points 
            MidSurfaceNameSpace.Component.Model model = new MidSurfaceNameSpace.Component.Model();
            MidSurfaceNameSpace.Primitive.Figure figure = new MidSurfaceNameSpace.Primitive.Figure();
            MidSurfaceNameSpace.Primitive.Contour countour = new MidSurfaceNameSpace.Primitive.Contour();
            for (int i = 0; i < list_of_points.Count; i += 4)
            {
                System.Collections.Generic.List<System.Windows.Point> pillars = new System.Collections.Generic.List<System.Windows.Point>
                {
                    list_of_points[i],
                    list_of_points[i+1],
                    list_of_points[i+2],
                    list_of_points[i+3],
                };
                MidSurfaceNameSpace.Primitive.Segment segment = new MidSurfaceNameSpace.Primitive.Segment(new MidSurfaceNameSpace.Primitive.BezierCurve(), new System.Collections.Generic.List<System.Windows.Point>(pillars));
                countour.Add(segment);
            }

            figure.Add(countour);
            model.Add(figure);
            return model;
        }

        [TestMethod]
        public void ViewTestWithCanvas()
        {
            MidSurfaceNameSpace.Component.Model model = CreateSomeModel(
                new System.Collections.Generic.List<System.Windows.Point>
                    {
                         new System.Windows.Point(0, 0),
                         new System.Windows.Point(10, 0),
                         new System.Windows.Point(10, 10),
                         new System.Windows.Point(0, 10),
                         new System.Windows.Point(0, 10),
                         new System.Windows.Point(-20, 20),
                         new System.Windows.Point(-20, 0),
                         new System.Windows.Point(0, 0)

                    });
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

            //Template for checking that all segments are into canvas and in model

            //Collecting date about segments 
            System.Collections.Generic.List<MidSurfaceNameSpace.Primitive.ISegment> list_segments = model.GetCanvasData().ToList<MidSurfaceNameSpace.Primitive.ISegment>();
            //Check count of segments on canvas and segments in model
            Assert.AreEqual(canvas.Children.Count, list_segments.Count);

            //Check every segment
            for (int i=0; i<list_segments.Count;++i)
            {
                //Get polyline data from canvas
                Polyline pl = canvas.Children[i] as Polyline;
                MidSurfaceNameSpace.Primitive.ISegment sgmnt = list_segments[i];
                //Check that begin and end points of current segment inside canvas
                Assert.IsTrue((pl.Points[0].X>=0) &&(pl.Points[0].X<=canvas.ActualWidth));
                Assert.IsTrue((pl.Points[0].Y >= 0) && (pl.Points[0].Y <= canvas.ActualHeight));
            }
         
        }
    }
}
