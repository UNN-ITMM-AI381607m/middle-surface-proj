using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using MidSurfaceNameSpace.Solver;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.UnitTests
{
    class FakeMSPointFinder : IMSPointFinder
    {
        public IMSPoint FindMSPoint(Point contourPoint, Normal normal)
        {
            return new MSPoint(new Point(contourPoint.X + normal.Dx() * 3, contourPoint.Y + normal.Dy() * 3), null);
        }
    }

    [TestClass]
    public class DetailizerTest
    {
        [TestMethod]
        public void DetailizeTest()
        {
            List<ISegment> segments = new List<ISegment>
            {
                new Segment(new BezierCurve(), new List<Point> { new Point(54, 54), new Point(54, 120) }),
                new Segment(new BezierCurve(), new List<Point> { new Point(54, 120), new Point(266, 120) }),
                new Segment(new BezierCurve(), new List<Point> { new Point(266, 120), new Point(266, 54) }),
                new Segment(new BezierCurve(), new List<Point> { new Point(266, 54), new Point(54, 54) })
            };

            List<ICustomLine> lines = new List<ICustomLine>
            {
               new CustomLine(new CustomPoint(0, 0, segments[0].GetCurvePoint(0)), new CustomPoint(0, 0.5, segments[0].GetCurvePoint(0.5))),
               new CustomLine (new CustomPoint(0, 0.5, segments[0].GetCurvePoint(0.5)), new CustomPoint(0, 1, segments[0].GetCurvePoint(1))),
               new CustomLine(new CustomPoint(1, 0, segments[1].GetCurvePoint(0)), new CustomPoint(1, 0.5, segments[1].GetCurvePoint(0.5))),
               new CustomLine (new CustomPoint(1, 0.5, segments[1].GetCurvePoint(0.5)), new CustomPoint(1, 1, segments[1].GetCurvePoint(1))),
               new CustomLine(new CustomPoint(2, 0, segments[2].GetCurvePoint(0)), new CustomPoint(2, 0.5, segments[2].GetCurvePoint(0.5))),
               new CustomLine (new CustomPoint(2, 0.5, segments[2].GetCurvePoint(0.5)), new CustomPoint(2, 1, segments[2].GetCurvePoint(1))),
               new CustomLine(new CustomPoint(3, 0, segments[3].GetCurvePoint(0)), new CustomPoint(3, 0.5, segments[3].GetCurvePoint(0.5))),
               new CustomLine (new CustomPoint(3, 0.5, segments[3].GetCurvePoint(0.5)), new CustomPoint(3, 1, segments[3].GetCurvePoint(1)))
            };

            IDetailizer detailizer = new Detailizer(new FakeMSPointFinder(), lines, segments, -1);
            var msPoints = detailizer.Detalize();
            Assert.AreEqual(lines.Count, msPoints.Count);

            detailizer = new Detailizer(new FakeMSPointFinder(), lines, segments, 0.01);
            msPoints = detailizer.Detalize();
            Assert.AreEqual(752, msPoints.Count);
        }
    }
}
