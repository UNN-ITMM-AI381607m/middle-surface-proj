using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Linq;
using MidSurfaceNameSpace.Solver;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class JoinMsPointsTest
    {
        [TestMethod]
        public void TestJoinMSPoints()
        {
            Graph msGraph = new Graph();

            msGraph.AddEdge(new Point(0, 0), new Point(0, 2));
            msGraph.AddEdge(new Point(0, 2), new Point(2, 2));
            msGraph.AddEdge(new Point(2, 2), new Point(2, 0));
            msGraph.AddEdge(new Point(2, 0), new Point(0, 0));

            var midsurface = new JoinMSPoints(msGraph).Join();

            // 4 edges = 4 segments of contour
            Assert.AreEqual(4, midsurface.GetData().Count());
        }
    }
}
