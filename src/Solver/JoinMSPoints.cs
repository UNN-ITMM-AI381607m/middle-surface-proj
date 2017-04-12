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

        public IMidSurface Join(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            if (mspointfinder == null) return null;

            IMidSurface midsurface = new MidSurfaceNameSpace.MidSurface();

            var points = JoinPoints(mspointfinder, mspoints, accuracy);
            for (int i = 0; i < points.Count(); i++)
            {
                int j = i == points.Count() - 1 ? 0 : i + 1;
                midsurface.Add(PointsToSegment(points[i], points[j]));
            }
            return midsurface;
        }


        private List<Point> JoinPoints(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            List<Point> result = new List<Point>();

            var segments = mspointfinder.GetSegments();
            double t1, t2;
            bool ignoreAccuracy = false;

            for (var i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;

                if ((mspoints[j].GetPoint() - mspoints[i].GetPoint()).Length <= accuracy ||
                     GetDistanceFromLines(mspoints[i], mspoints[j]) <= accuracy/4 ||
                     ignoreAccuracy)
                {
                    result.Add(mspoints[i].GetPoint());
                    ignoreAccuracy = false;
                }
                else
                {
                    var isPoint1FromBisector = mspoints[i].GetAdditionLine() != null;
                    var currentLine = isPoint1FromBisector ?
                        mspoints[i].GetAdditionLine() :
                        mspoints[i].GetLine();

                    var isPoint2FromBisector = mspoints[j].GetAdditionLine() != null;
                    var nextLine = mspoints[j].GetLine();

                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        if (isPoint1FromBisector)
                        {
                            t1 = currentLine.GetPoint1().GetT();
                            t2 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;

                            if (Math.Abs(t2 - t1) <= 0.001)
                            {
                                ignoreAccuracy = true;
                                continue;
                            }

                            var additionLine = new CustomLine
                                (
                                     currentLine.GetPoint1(),
                                     new CustomPoint(currentLine.GetPoint1().GetN(),
                                                    t2,
                                                    segments[currentLine.GetPoint1().GetN()].GetCurvePoint(t2))
                                );

                            var msPointFromBisector = new MSPoint(mspoints[i].GetPoint(), mspoints[i].GetLine(), additionLine);
                            mspoints[i] = msPointFromBisector;
                        }
                        else if (isPoint2FromBisector)
                        {
                            t1 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;
                            t2 = nextLine.GetPoint2().GetT();

                            if (Math.Abs(t2 - t1) <= 0.001)
                            {
                                ignoreAccuracy = true;
                                continue;
                            }

                            var line = new CustomLine
                                (
                                    new CustomPoint(nextLine.GetPoint1().GetN(),
                                                    t1,
                                                    segments[nextLine.GetPoint1().GetN()].GetCurvePoint(t1)),
                                     nextLine.GetPoint2()
                                );

                            var msPointFromBisector = new MSPoint(mspoints[j].GetPoint(), line, mspoints[j].GetAdditionLine());
                            mspoints[j] = msPointFromBisector;
                            currentLine = nextLine;
                        }
                        else
                        {
                                t1 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;
                                t2 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;

                                if (Math.Abs(t2 - t1) <= 0.01)
                                {
                                    ignoreAccuracy = true;
                                    continue;
                                }

                            currentLine = new CustomLine
                                (
                                    new CustomPoint(currentLine.GetPoint1().GetN(),
                                                    t1,
                                                    segments[currentLine.GetPoint1().GetN()].GetCurvePoint(t1)),

                                    new CustomPoint(nextLine.GetPoint1().GetN(),
                                                    t2,
                                                   segments[nextLine.GetPoint1().GetN()].GetCurvePoint(t2))
                                );
                        }
                        var newPoints = mspointfinder.FindMSPoints(new List<ICustomLine>() { currentLine });
                            mspoints.Insert(i + 1, newPoints[0]);
                    }
                    else
                    {
                        var bisector = currentLine.GetRightNormal() + nextLine.GetRightNormal();
                        bisector.Normalize();

                        t1 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;
                        t2 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;

                        if (Math.Abs(t2 - t1) <= 0.001)
                        {
                            ignoreAccuracy = true;
                            continue;
                        }

                        currentLine = new CustomLine
                            (
                                new CustomPoint(currentLine.GetPoint1().GetN(),
                                                t1,
                                                segments[currentLine.GetPoint1().GetN()].GetCurvePoint(t1)),
                                currentLine.GetPoint2()
                            );

                        nextLine = new CustomLine
                           (
                               nextLine.GetPoint1(),

                               new CustomPoint(nextLine.GetPoint1().GetN(),
                                               t2,
                                               segments[nextLine.GetPoint1().GetN()].GetCurvePoint(t2))
                           );

                        var msPoint = mspointfinder.GetMSPoint(bisector, nextLine.GetPoint1().GetPoint(), currentLine);
                        var msPointFromBisector = new MSPoint(msPoint.GetPoint(), currentLine, nextLine);
                        mspoints.Insert(i + 1, msPointFromBisector);
                    }
                    i--;
                }
            }
            return result;
        }


        private double GetDistanceFromLines(IMSPoint mspoint1, IMSPoint mspoint2)
        {
            Point point1, point2;
            if (mspoint1.GetAdditionLine() != null)
            {
                point1 = mspoint1.GetAdditionLine().GetPoint1().GetPoint();
                point2 = mspoint2.GetLine().GetPoint2().GetPoint();
            }
            else
            {
                point1 = mspoint1.GetLine().GetPoint1().GetPoint();
                point2 = mspoint2.GetLine().GetPoint2().GetPoint();
            }
            return (point1 - point2).Length;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }

    }
}
