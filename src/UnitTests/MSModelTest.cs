using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.IO;
using MidSurfaceNameSpace.Component;
using System.Windows;

namespace UnitTests
{
    [TestClass]
    public class MSModelTest
    {
        [TestMethod]
        public void TestCanvasDataRemove()
        {
            List<Point> pillar = new List<Point>();
            Point p1 = new Point();
            p1.X = 1;
            p1.Y = 1;
            Point p2 = new Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            contour.Add(segment);
            IFigure figure = new Figure();
            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>();
            removed.Add(true);
            msmodel.Removed = removed;
            int counter = msmodel.GetCanvasData().Count();
            Assert.AreEqual(counter, 0);
        }

        [TestMethod]
        public void TestCanvasDataWithOutSegments()
        {
            IFigure figure = new Figure();
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            List<Point> pillar = new List<Point>();
            Point p1 = new Point();
            p1.X = 1;
            p1.Y = 1;
            Point p2 = new Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 3;
            p1.Y = 3;
            p2 = new Point();
            p2.X = 4;
            p2.Y = 4;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            figure.Add(contour);

            contour = new MidSurfaceNameSpace.Primitive.Contour();
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2 = new Point();
            p2.X = 6;
            p2.Y = 6;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 7;
            p1.Y = 7;
            p2 = new Point();
            p2.X = 8;
            p2.Y = 8;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);

            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            IEnumerable<ISegment> ccc = model.GetCanvasData();
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>() { true, false, false, true };
            msmodel.Removed = removed;
            int counter = msmodel.GetCanvasData().Count();
            List<ISegment> ccc1 = msmodel.GetCanvasData().ToList();
            Assert.AreEqual(counter, 2);
            Assert.AreEqual(ccc1[0].GetPillar()[0], new Point(3, 3));
            Assert.AreEqual(ccc1[0].GetPillar()[1], new Point(4, 4));
            Assert.AreEqual(ccc1[1].GetPillar()[0], new Point(5, 5));
            Assert.AreEqual(ccc1[1].GetPillar()[1], new Point(6, 6));
        }

        [TestMethod]
        public void TestCanvasDataWithOutCountour()
        {
            IFigure figure = new Figure();
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            List<Point> pillar = new List<Point>();
            Point p1 = new Point();
            p1.X = 1;
            p1.Y = 1;
            Point p2 = new Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 3;
            p1.Y = 3;
            p2 = new Point();
            p2.X = 4;
            p2.Y = 4;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            figure.Add(contour);

            contour = new MidSurfaceNameSpace.Primitive.Contour();
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2 = new Point();
            p2.X = 6;
            p2.Y = 6;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 7;
            p1.Y = 7;
            p2 = new Point();
            p2.X = 8;
            p2.Y = 8;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);

            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            IEnumerable<ISegment> ccc = model.GetCanvasData();
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>() { true, false, true, true };
            msmodel.Removed = removed;
            List<ISegment> ccc1 = msmodel.GetCanvasData().ToList();
            int counter = msmodel.GetCanvasData().Count();
            Assert.AreEqual(counter, 1);
            Assert.AreEqual(ccc1[0].GetPillar()[0], new Point(3, 3));
            Assert.AreEqual(ccc1[0].GetPillar()[1], new Point(4, 4));
        }

        [TestMethod]
        public void TestDataWithOutContour()
        {
            IFigure figure = new Figure();
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            List<Point> pillar = new List<Point>();
            Point p1 = new Point();
            p1.X = 1;
            p1.Y = 1;
            Point p2 = new Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 3;
            p1.Y = 3;
            p2 = new Point();
            p2.X = 4;
            p2.Y = 4;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            figure.Add(contour);

            contour = new MidSurfaceNameSpace.Primitive.Contour();
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2 = new Point();
            p2.X = 6;
            p2.Y = 6;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 7;
            p1.Y = 7;
            p2 = new Point();
            p2.X = 8;
            p2.Y = 8;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);

            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            IEnumerable<ISegment> ccc = model.GetCanvasData();
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>() { true, false, true, true };
            msmodel.Removed = removed;
            List<IFigure> fig = msmodel.GetData().ToList();
            List<ISegment> ccc1 = new List<ISegment>();
            foreach (var f in fig)
            {
                foreach (var c in f.GetContours())
                {
                    ccc1.AddRange(c.GetSegments());
                }
            }
            int counter = msmodel.GetCanvasData().Count();
            Assert.AreEqual(counter, 1);
            Assert.AreEqual(ccc1[0].GetPillar()[0], new Point(3, 3));
            Assert.AreEqual(ccc1[0].GetPillar()[1], new Point(4, 4));
        }

        [TestMethod]
        public void TestDataWithOutSegments()
        {
            IFigure figure = new Figure();
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            List<Point> pillar = new List<Point>();
            Point p1 = new Point();
            p1.X = 1;
            p1.Y = 1;
            Point p2 = new Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 3;
            p1.Y = 3;
            p2 = new Point();
            p2.X = 4;
            p2.Y = 4;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            figure.Add(contour);

            contour = new MidSurfaceNameSpace.Primitive.Contour();
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 5;
            p1.Y = 5;
            p2 = new Point();
            p2.X = 6;
            p2.Y = 6;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);
            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 7;
            p1.Y = 7;
            p2 = new Point();
            p2.X = 8;
            p2.Y = 8;
            pillar.Add(p1);
            pillar.Add(p2);
            segment = new Segment(new BezierCurve(), pillar);
            contour.Add(segment);

            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            IEnumerable<ISegment> ccc = model.GetCanvasData();
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>() { true, false, false, true };
            msmodel.Removed = removed;
            List<IFigure> fig = msmodel.GetData().ToList();
            List<ISegment> ccc1 = new List<ISegment>();
            foreach (var f in fig)
            {
                foreach (var c in f.GetContours())
                {
                    ccc1.AddRange(c.GetSegments());
                }
            }
            int counter = msmodel.GetCanvasData().Count();
            Assert.AreEqual(counter, 2);
            Assert.AreEqual(ccc1[0].GetPillar()[0], new Point(3, 3));
            Assert.AreEqual(ccc1[0].GetPillar()[1], new Point(4, 4));
            Assert.AreEqual(ccc1[1].GetPillar()[0], new Point(5, 5));
            Assert.AreEqual(ccc1[1].GetPillar()[1], new Point(6, 6));
        }
    }
}
