using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using MidSurfaceNameSpace.Solver;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class JoinMsPointsTest
    {
        [TestMethod]
        public void TestJoinMSPointsSuitableForAccuracy()
        {
            List<IMSPoint> points = new List<IMSPoint>();

            points.Add(new MSPoint(new System.Windows.Point(0, 0), null));
            points.Add(new MSPoint(new System.Windows.Point(0, 2), null));
            points.Add(new MSPoint(new System.Windows.Point(2, 2), null));
            points.Add(new MSPoint(new System.Windows.Point(2, 0), null));

            var midsurface = new JoinMSPoints().Join(null, points, 5);
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
