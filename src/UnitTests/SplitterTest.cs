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
        List<ISegment> segments;
        Splitter splitter;

        public SplitterTest()
        {
            segments = new List<ISegment>
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
            splitter = new Splitter();
        }

        [TestMethod]
        public void TestSplitMethodReturnsNotNull()
        {
            List<ICustomLine> lines = splitter.Split(segments, 0.17);

            Assert.AreNotEqual(0, lines.Count);
        }

        [TestMethod]
        public void TestSplitLinesContainJointsOfSegments()
        {
            List<ICustomLine> lines = splitter.Split(segments, 0.17);
            List<System.Windows.Point> joints = GetJoints(segments);
            List<System.Windows.Point> jointLines = GetJoints(lines);
            foreach (var point in jointLines)
            {
                joints.Remove(point);
            }

            Assert.AreEqual(0, joints.Count);
        }

        List<System.Windows.Point> GetJoints(List<ISegment> segments)
        {
            List<System.Windows.Point> joints = new List<System.Windows.Point>();
            foreach (var segment in segments)
            {
                joints.AddRange(segment.GetPillar());
            }
            return joints;
        }

        List<System.Windows.Point> GetJoints(List<ICustomLine> lines)
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
