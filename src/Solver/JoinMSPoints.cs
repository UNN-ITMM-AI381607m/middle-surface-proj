using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        public JoinMSPoints() { }

        public IMidSurface Join(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            if (mspointfinder == null) return null;

            IMidSurface midsurface = new MidSurfaceNameSpace.MidSurface();
    
            var points = JoinPoints(mspointfinder, mspoints, accuracy);
            for (int i = 0; i < points.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;
                midsurface.Add(PointsToSegment(points[i], points[j]));
            }
            return midsurface;
        }
        

        private List<Point> JoinPoints(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            List<Point> result = new List<Point>();

            var segments = mspointfinder.GetSegments();

            for (var i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;

                if ((mspoints[j].GetPoint() - mspoints[i].GetPoint()).Length <= accuracy)
                {
                    result.Add(mspoints[i].GetPoint());
                }
                else
                {
                    var currentLine = mspoints[i].GetLine();
                    var nextLine = mspoints[j].GetLine();

                    List<ICustomLine> lines = new List<ICustomLine>();

                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        var t1 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;
                        var t2 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;

                        currentLine = new CustomLine
                            (
                                new CustomPoint(currentLine.GetPoint1().GetN(),
                                                t1,
                                                segments[currentLine.GetPoint1().GetN()].GetCurvePoint(t1)),

                                new CustomPoint(nextLine.GetPoint1().GetN(),
                                                t2,
                                                segments[nextLine.GetPoint1().GetN()].GetCurvePoint(t2))
                            );

                        var newPoints = mspointfinder.FindMSPoints(new List<ICustomLine>() { currentLine });
                        mspoints.Insert(i + 1, newPoints[0]);
                    }
                    else
                    {
                        var bisector = currentLine.GetRightNormal() + nextLine.GetRightNormal();
                        bisector.Normalize();
                        var msPoint = mspointfinder.GetMSPoint(bisector, nextLine.GetPoint1().GetPoint(), nextLine);
                        mspoints.Insert(i + 1, msPoint);
                    }
                    i--;
                }
            }
            return result;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        } 
    }
}
