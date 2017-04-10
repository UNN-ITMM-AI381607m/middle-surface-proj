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

                    int newPointsSize = 0;

                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        currentLine = new CustomLine
                            (
                                new CustomPoint(currentLine.GetPoint1().GetN(),
                                                Math.Abs(currentLine.GetPoint2().GetT() - currentLine.GetPoint1().GetT()) / 2,
                                                segments[currentLine.GetPoint1().GetN()].GetCurvePoint(currentLine.GetPoint1().GetT())),
                                                
                                new CustomPoint(nextLine.GetPoint1().GetN(),
                                                Math.Abs(nextLine.GetPoint2().GetT() - nextLine.GetPoint1().GetT()) / 2,
                                                segments[nextLine.GetPoint1().GetN()].GetCurvePoint(nextLine.GetPoint1().GetT()))
                            );

                        lines.Add(currentLine);

                        var newPoints = mspointfinder.FindMSPoints(lines);
                        newPointsSize = newPoints.Count();
                        for (int k = 0; k < newPointsSize; k++)
                        {
                            mspoints.Insert(i + 1 + k, newPoints[k]);
                        }
                    }
                    else
                    {
                        var bisector = currentLine.GetRightNormal() + nextLine.GetRightNormal();
                        bisector.Normalize();
                        var msPoint = mspointfinder.GetMSPoint(bisector, nextLine.GetPoint1().GetPoint(), nextLine);
                        newPointsSize = 1;
                        mspoints.Insert(i + 1, msPoint);
                    }

                    if(newPointsSize > 0) i--;
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
