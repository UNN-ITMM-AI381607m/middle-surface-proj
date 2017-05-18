using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Solver;
using System.Collections.Generic;
using System.Windows;
using MidSurfaceNameSpace.Primitive;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class MSPointFinderTest
    {
        MSPointFinder finder;

        List<ICustomLine> square5x5 = new List<ICustomLine>()
            {
                new CustomLine(new CustomPoint(0, 0, new Point(0, 0)), new CustomPoint(0, 1, new Point(0, 5))),
                new CustomLine(new CustomPoint(1, 0, new Point(0, 5)), new CustomPoint(1, 1, new Point(5, 5))),
                new CustomLine(new CustomPoint(2, 0, new Point(5, 5)), new CustomPoint(2, 1, new Point(5, 0))),
                new CustomLine(new CustomPoint(3, 0, new Point(5, 0)), new CustomPoint(3, 1, new Point(0, 0)))
            };

        public MSPointFinderTest()
        {
        }

        [TestMethod]
        public void FindMSPointsForSquareAreInCenter()
        {
            finder = new MSPointFinder(square5x5);
            Point center = new Point(2.5, 2.5);
            double accuracy = 0.0001;

            List<IMSPoint> msPoints = new List<IMSPoint>();

            msPoints.Add(finder.FindMSPoint(new Point(0, 2.5), new Normal(null, 0, 1, 0)));
            msPoints.Add(finder.FindMSPoint(new Point(2.5, 5), new Normal(null, 0, 0, -1)));
            msPoints.Add(finder.FindMSPoint(new Point(5, 2.5), new Normal(null, 0, -1, 0)));
            msPoints.Add(finder.FindMSPoint(new Point(2.5, 0), new Normal(null, 0, 0, 1)));

            double xAverage = msPoints.Sum(x => x.GetPoint().X) / msPoints.Count;
            double yAverage = msPoints.Sum(x => x.GetPoint().Y) / msPoints.Count;

            Point averagePoint = new Point(xAverage, yAverage);

            Assert.IsTrue(CustomPoint.ClosePoints(averagePoint, center, accuracy));

            foreach (var msPoint in msPoints)
            {
                Assert.IsTrue(CustomPoint.ClosePoints(msPoint.GetPoint(), center, accuracy));
            }
        }
    }
}
