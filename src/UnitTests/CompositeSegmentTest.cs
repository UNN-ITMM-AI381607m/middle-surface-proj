using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.IO;
using MidSurfaceNameSpace.Component;
using UnitTests;
using System.Windows;

namespace UnitTests
{
    [TestClass]
    public class CompositeSegmentTest
    {
        [TestMethod]
        public void TestComposite()
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

            pillar = new List<Point>();
            p1 = new Point();
            p1.X = 3;
            p1.Y = 3;
            p2 = new Point();
            p2.X = 4;
            p2.Y = 4;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment2 = new Segment(new BezierCurve(), pillar);

            ISegment compositeSegment = new CompositeSegment();
            compositeSegment.Add(segment);
            compositeSegment.Add(segment2);
            Assert.AreEqual(compositeSegment.GetPillar().Count(), 4);
            Assert.AreEqual(compositeSegment.GetCurvePoint(0.48), segment.GetCurvePoint(0.96));
            Assert.AreEqual(compositeSegment.GetCurvePoint(0.75), segment2.GetCurvePoint(0.5));
        }
    }
}
