using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Solver;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class SplitterTest
    {
        List<IContour> contours;
        Splitter splitter;

        public SplitterTest()
        {
            contours = new List<IContour>();
            Primitive.Contour contour = new Primitive.Contour();
            
            List<ISegment> segments = new List<ISegment>
            {
                new Segment(new BezierCurve(), new List<System.Windows.Point>
                {
                    new System.Windows.Point(0, 0),
                    new System.Windows.Point(0, 1)
                }),
                new Segment(new BezierCurve(), new List<System.Windows.Point>
                {
                    new System.Windows.Point(0, 1),
                    new System.Windows.Point(1, 0)
                }),
                new Segment(new BezierCurve(), new List<System.Windows.Point>
                {
                    new System.Windows.Point(1, 0),
                    new System.Windows.Point(0, 0)
                })
            };

            foreach (var segment in segments)
            {
                contour.Add(segment);
            }
            contours.Add(contour);
            
            splitter = new Splitter();
        }

        [TestMethod]
        public void TestSplitMethodReturnsNotNull()
        {
            List<ICustomLine> lines = splitter.Split(contours, 0.17);

            Assert.AreNotEqual(0, lines.Count);
        }

        [TestMethod]
        public void TestSplitLinesContainJointsOfSegments()
        {
            List<ICustomLine> lines = splitter.Split(contours, 0.17);
            List<System.Windows.Point> joints = new List<System.Windows.Point>();
            foreach (var contour in contours)
            {
                joints.AddRange(GetJoints(contour.GetSegments()));
            }
            List<System.Windows.Point> jointLines = GetJoints(lines);
            foreach (var point in jointLines)
            {
                joints.Remove(point);
            }

            Assert.AreEqual(0, joints.Count);
        }

        [TestMethod]
        public void TestBoundaryAccuracy()
        {
            List<ICustomLine> lines = splitter.Split(contours, 1);
            int segmentsCount = 0;
            foreach (var contour in contours)
            {
                segmentsCount += contour.GetSegments().Count();
            }
            Assert.AreEqual(segmentsCount, lines.Count);
        }

        List<System.Windows.Point> GetJoints(IEnumerable<ISegment> segments)
        {
            List<System.Windows.Point> joints = new List<System.Windows.Point>();
            foreach (var segment in segments)
            {
                joints.AddRange(segment.GetPillar());
            }
            return joints;
        }

        List<System.Windows.Point> GetJoints(IEnumerable<ICustomLine> lines)
        {
            List<System.Windows.Point> joints = new List<System.Windows.Point>();
            foreach (var line in lines)
            {
                joints.Add(line.GetPoint1().GetPoint());
                joints.Add(line.GetPoint2().GetPoint());
            }
            return joints;
        }
    }
}
