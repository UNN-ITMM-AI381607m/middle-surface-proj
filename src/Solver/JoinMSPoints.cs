using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        private List<IMSPoint> msPoints;
        private List<ICustomLine> simplifiedModel;
        private Graph graph;

        const double lineAccuracy = 0.1;
        const double radiusAddition = 0.00001;

        public JoinMSPoints(IEnumerable<IMSPoint> points, IEnumerable<ICustomLine> lines = null)
        {
            msPoints = points.ToList();
            if (lines != null)
                simplifiedModel = lines.ToList();
            graph = new Graph();
        }

        public IMidSurface Join()
        {
            IMidSurface midsurface = new MidSurface();
            List<ISegment> segments = new List<ISegment>();

            //Implementation of new algorithm based on marks (not working on all tests)

            //FindPath();
            //List<int> connectionOrder = new List<int>();
            //foreach (var line in simplifiedModel)
            //{
            //    var marksOnLine = line.GetMarks();
            //    foreach (var mark in marksOnLine)
            //    {
            //        //for debug only
            //        //var normal = line.GetRightNormal();
            //        //midsurface.Add(PointsToSegment(Vector.Add(normal, mark.ContactPoint),
            //        //    Vector.Add(-normal, mark.ContactPoint)));
            //        if (connectionOrder.Count == 0 || mark.MSPointIndex != connectionOrder.Last())
            //            connectionOrder.Add(mark.MSPointIndex);
            //    }
            //}

            //for (int i = 0; i < connectionOrder.Count - 1; i++)
            //{
            //    int j = i + 1 == connectionOrder.Count ? 0 : i + 1;
            //    graph.AddEdge(msPoints[connectionOrder[i]].GetPoint(), msPoints[connectionOrder[j]].GetPoint());
            //}

            //graph.RemoveCycles();

            for (int i = 0; i < msPoints.Count; i++)
            {
                int j = i + 1 == msPoints.Count ? 0 : i + 1;
                graph.AddEdge(msPoints[i].GetPoint(), msPoints[j].GetPoint());
            }

            foreach (var s in graph.GetEdges())
            {
                midsurface.Add(PointsToSegment(s.vertex1.point, s.vertex2.point));
            }

            //For debug only
            //foreach (var line in simplifiedModel)
            //{
            //    var normal = line.GetRightNormal();
            //    midsurface.Add(PointsToSegment(Vector.Add(2*normal, line.GetPoint1().GetPoint()),
            //        Vector.Add(-2*normal, line.GetPoint1().GetPoint())));
            //}

            return midsurface;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }

        void FindPath()
        {
            int counter = 0;
            foreach (var msPoint in msPoints)
            {
                Point center = msPoint.GetPoint();
                Point linePoint1 = msPoint.GetLine().GetPoint1().GetPoint();
                Point linePoint2 = msPoint.GetLine().GetPoint2().GetPoint();
                Point middlePoint = Vector.Add((linePoint2 - linePoint1) / 2, linePoint1);
                double R = (center - middlePoint).Length + radiusAddition;
                SetMarks(center, R, counter);
                counter++;
            }
        }

        void SetMarks(Point center, double R, int id)
        {
            for (int i = 0; i < simplifiedModel.Count; i++)
            {
                Point intersecPoint1 = new Point();
                Point intersecPoint2 = new Point();
                int intersecCount = CustomLine.LineSegmentIntersectionCircle(center, R, simplifiedModel[i].GetPoint1().GetPoint(),
                    simplifiedModel[i].GetPoint2().GetPoint(), ref intersecPoint1, ref intersecPoint2);
                if (intersecCount == 1)
                {
                    simplifiedModel[i].AddMark(id, intersecPoint1);
                }
                else if (intersecCount == 2)
                {
                    Point contactPoint = Vector.Add((intersecPoint2 - intersecPoint1) / 2, intersecPoint1);
                    simplifiedModel[i].AddMark(id, contactPoint);
                }
            }
        }
    }
}
