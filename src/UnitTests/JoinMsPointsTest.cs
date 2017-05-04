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
        [TestMethod]
        public void TestJoinMSPoints()
        {
            Graph msGraph = new Graph();

            msGraph.AddEdge(new System.Windows.Point(0, 0), new System.Windows.Point(0, 2));
            msGraph.AddEdge(new System.Windows.Point(0, 2), new System.Windows.Point(2, 2));
            msGraph.AddEdge(new System.Windows.Point(2, 2), new System.Windows.Point(2, 0));
            msGraph.AddEdge(new System.Windows.Point(2, 0), new System.Windows.Point(0, 0));

            var midsurface = new JoinMSPoints(msGraph).Join();

            // 4 edges = 4 segments of contour
            Assert.AreEqual(4, midsurface.GetData().Count());
        }
    }
}
