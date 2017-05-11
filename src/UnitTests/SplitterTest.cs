using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Solver;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class SplitterTest
    {
        List<IContour> contours;
        Splitter splitter;
        int numOfSegments;

        public SplitterTest()
        {
            contours = new List<IContour>();
            Primitive.Contour contour1 = new Primitive.Contour();
            Primitive.Contour contour2 = new Primitive.Contour();

            List<ISegment> segments1 = new List<ISegment>
            {
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(-2, -2),
                    new Point(-2, 2)
                }),
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(-2, 2),
                    new Point(2, 2)
                }),
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(2, 2),
                    new Point(2, -2)
                }),
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(2, -2),
                    new Point(-2, -2)
                })
            };

            List<ISegment> segments2 = new List<ISegment>
            {
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(0, 0),
                    new Point(1, 0)
                }),
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(1, 0),
                    new Point(0, 1)
                }),
                new Segment(new BezierCurve(), new List<Point>
                {
                    new Point(0, 1),
                    new Point(0, 0)
                })
            };

            foreach (var segment in segments1)
            {
                contour1.Add(segment);
            }
            foreach (var segment in segments2)
            {
                contour2.Add(segment);
            }
            contours.Add(contour1);
            contours.Add(contour2);
            
            splitter = new Splitter();
            numOfSegments = 0;
            foreach (var contour in contours)
            {
                numOfSegments += contour.GetSegments().Count();
            }
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
            List<Point> joints = new List<Point>();
            foreach (var contour in contours)
            {
                joints.AddRange(GetJoints(contour.GetSegments()));
            }
            List<Point> jointLines = GetJoints(lines);
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

        [TestMethod]
        public void TestAllSegmentsHaveUniqueNumber()
        {
            List<ICustomLine> lines = splitter.Split(contours, 0.5);
            List<int> numbers = new List<int>();
            foreach (var line in lines)
            {
                numbers.Add(line.GetPoint1().GetN());
                numbers.Add(line.GetPoint2().GetN());
            }
            Assert.AreEqual(numbers.Distinct().Count(), numOfSegments);
        }

        List<Point> GetJoints(IEnumerable<ISegment> segments)
        {
            List<Point> joints = new List<Point>();
            foreach (var segment in segments)
            {
                joints.AddRange(segment.GetPillar());
            }
            return joints;
        }

        List<Point> GetJoints(IEnumerable<ICustomLine> lines)
        {
            List<Point> joints = new List<Point>();
            foreach (var line in lines)
            {
                joints.Add(line.GetPoint1().GetPoint());
                joints.Add(line.GetPoint2().GetPoint());
            }
            return joints;
        }
    }
}
