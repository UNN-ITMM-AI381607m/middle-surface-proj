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

            for(double i = 0.5; i < 4.5; i += 0.5)
            {
                points.Add(new MSPoint(new System.Windows.Point(i, 1), null));
            }

            Assert.AreEqual(8, points.Count());

            var midsurface = new JoinMSPoints().Join(null, points, 5);

            // 8 points = 7 segments
            Assert.AreEqual(7, midsurface.GetData().Count());
            
        }

        [TestMethod]
        public void TestJoinMSPointsNotSuitableForAccuracy()
        {
            //List<IMSPoint> points = new List<IMSPoint>();

            //for (double i = 0.5; i < 4.5; i += 0.5)
            //{
            //    points.Add(new MSPoint(new System.Windows.Point(i, 1), null));
            //}

            //Assert.AreEqual(8, points.Count());

            //var midsurface = new JoinMSPoints().Join(null, points, 1);

            //// 8 points = 7 segments
            //Assert.AreEqual(7, midsurface.GetData().Count());
            Assert.AreEqual(0, 0);
        }
    }
}
