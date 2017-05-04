using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        private Graph graph;

        public JoinMSPoints(Graph graph)
        {
            this.graph = graph;
        }

        public IMidSurface Join()
        {
            IMidSurface midsurface = new MidSurface();

            foreach (var s in graph.GetEdges())
            {
                midsurface.Add(PointsToSegment(s.vertex1.point, s.vertex2.point));
            }

            return midsurface;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }
    }
}
