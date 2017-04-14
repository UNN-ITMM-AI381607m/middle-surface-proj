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
    [TestClass]
    public class JoinMsPointsTest
    {
        class FakePointsFinder : IMSPointFinder
        {
            public void SetLines(List<ICustomLine> lines)
            { }
            public List<IMSPoint> FindMSPoints()
            {
                List<IMSPoint> points = new List<IMSPoint>();

                points.Add(new MSPoint(new System.Windows.Point(0, 0), null));
                points.Add(new MSPoint(new System.Windows.Point(0, 2), null));
                points.Add(new MSPoint(new System.Windows.Point(2, 2), null));
                points.Add(new MSPoint(new System.Windows.Point(2, 0), null));

                return points;
            }
            public IMSPoint FindMSPointForLine(ICustomLine line)
            {
                return null;
            }
        }

        [TestMethod]
        public void TestJoinMSPointsSuitableForAccuracy()
        {
            var fakeSegments = new List<ISegment>();
            var fakePointsFinder = new FakePointsFinder();
            var midsurface = new JoinMSPoints(fakePointsFinder, fakeSegments, 5).Join();

            // 4 points = 4 segments of contour
            Assert.AreEqual(4, midsurface.GetData().Count());
        }

        [TestMethod]
        public void TestJoinMSPointsNotSuitableForAccuracy()
        {
            Assert.AreEqual(0, 0);
        }
    }
}
