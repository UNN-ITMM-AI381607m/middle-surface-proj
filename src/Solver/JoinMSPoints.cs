﻿using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        public JoinMSPoints()
        {}

        public IMidSurface Join(Graph graph)
        {
            IMidSurface midsurface = new MidSurface();

            foreach (var s in graph.GetEdges())
            {
                midsurface.Add(PointsToSegment(s.vertex1.point, s.vertex2.point));
            }

            return midsurface;
        }

        public static ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }

        public static ISegment PointsToMSSegment(IMSPoint begin, IMSPoint end)
        {
            return new MSSegment(new BezierCurve(), new List<IMSPoint> { begin, end });
        }

        //for debug
        public IMidSurface Join(List<IMSPoint> points)
        {
            IMidSurface midsurface = new MidSurface();
            for (int i = 0; i < points.Count - 1; i++)
            {
                int j = i == points.Count - 1 ? 0 : i + 1;
                midsurface.Add(PointsToMSSegment(points[i], points[j]));
            }
            return midsurface;
        }

        public IMidSurface Join(List<Point> points)
        {
            IMidSurface midsurface = new MidSurface();
            for (int i = 0; i < points.Count; i++)
            {
                int j = i == points.Count - 1 ? 0 : i + 1;
                midsurface.Add(PointsToSegment(points[i], points[j]));
            }
            return midsurface;
        }
    }
}
