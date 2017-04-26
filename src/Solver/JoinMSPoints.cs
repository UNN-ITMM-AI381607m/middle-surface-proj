using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        private List<IMSPoint> mspoints;

        public JoinMSPoints(List<IMSPoint> points)
        {
            mspoints = points;
        }

        public IMidSurface Join()
        {
            IMidSurface midsurface = new MidSurface();
            for (int i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;
                //if (mspoints[i].GetLine().GetPoint2().GetPoint() != mspoints[j].GetLine().GetPoint1().GetPoint())
                //    continue;
                midsurface.Add(PointsToSegment(mspoints[i].GetPoint(), mspoints[j].GetPoint()));
            }
            return midsurface;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }
    }
}
