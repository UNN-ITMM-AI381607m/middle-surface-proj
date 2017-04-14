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
        internal class Accuracy
        {
            private double accuracy;

            public Accuracy(double accuracy)
            {
                this.accuracy = accuracy;
            }

            public bool IsComplianced(IMSPoint point1, IMSPoint point2)
            {
                var isComplianced = 
                    (point2.GetPoint() - point1.GetPoint()).Length <= accuracy ||
                    GetDistanceFromLines(point1, point2) <= accuracy / 4;

                return isComplianced;
            }

            private double GetDistanceFromLines(IMSPoint mspoint1, IMSPoint mspoint2)
            {
                var point1 = mspoint1.GetLine().GetPoint1().GetPoint();
                var point2 = mspoint2.GetLine().GetPoint2().GetPoint();
                return (point1 - point2).Length;
            }
        }

        private List<ISegment> segments;
        private IMSPointFinder msPointFinder;
        private Accuracy accuracy;

        public JoinMSPoints(IMSPointFinder finder, List<ISegment> segments, double accuracy)
        {
            this.segments = segments;
            this.accuracy = new Accuracy(accuracy);
            this.msPointFinder = finder;
        }

        public IMidSurface Join()
        {
            IMidSurface midsurface = new MidSurface();
            var mspoints = Qualify(msPointFinder.FindMSPoints());
            for (int i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;
                midsurface.Add(PointsToSegment(mspoints[i].GetPoint(), mspoints[j].GetPoint()));
            }
            return midsurface;
        }

        List<IMSPoint> Qualify(List<IMSPoint> mspoints)
        {
            var lines = new List<ICustomLine>();

            foreach (var point in mspoints)
            {
                lines.Add(point.GetLine());
            }
            for (int i = 0; i < mspoints.Count(); i++)
            {

                int j = i == mspoints.Count() - 1 ? 0 : i + 1;

                if (!accuracy.IsComplianced(mspoints[i], mspoints[j]))
                {
                    var currentLine = mspoints[i].GetLine();
                    var nextLine = mspoints[j].GetLine();
                    if (currentLine.GetPoint2().GetPoint() != nextLine.GetPoint1().GetPoint())
                        continue;

                    ICustomPoint point1 = null, point2 = null;

                    var vector1 = currentLine.GetPoint1().GetPoint() - currentLine.GetPoint2().GetPoint();
                    var vector2 = nextLine.GetPoint2().GetPoint() - nextLine.GetPoint1().GetPoint();
                    if (Vector.AngleBetween(vector1, vector2) >= 0 &&
                        currentLine.GetPoint1().GetN() != nextLine.GetPoint2().GetN())
                    {
                        continue;
                    }
                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        var t1 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;
                        var t2 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;

                        point1 = new CustomPoint(currentLine.GetPoint2().GetN(), t1,
                            segments[currentLine.GetPoint2().GetN()].GetCurvePoint(t1));
                        point2 = new CustomPoint(nextLine.GetPoint1().GetN(), t2, 
                            segments[nextLine.GetPoint1().GetN()].GetCurvePoint(t2));
                    }
                    else
                    {
                        point1 = new CustomPoint(currentLine.GetPoint2().GetN(), currentLine.GetPoint2().GetT(),
                            Vector.Add(currentLine.GetRightNormal() * 0.01, currentLine.GetPoint2().GetPoint()));
                        point2 = new CustomPoint(nextLine.GetPoint1().GetN(), nextLine.GetPoint1().GetT(),
                            Vector.Add(nextLine.GetRightNormal() * 0.01, nextLine.GetPoint1().GetPoint()));
                    }

                    if ((point1.GetPoint() - point2.GetPoint()).Length < 0.01)
                    {
                        continue;
                    }

                    var line1 = new CustomLine(currentLine.GetPoint1(), point1);
                    var line2 = new CustomLine(point1, point2);
                    var line3 = new CustomLine(point2, nextLine.GetPoint2());

                    int index = lines.IndexOf(currentLine);
                    if (lines.Remove(currentLine) && lines.Remove(nextLine))
                    {
                        if (lines.Count() > index)
                        {
                            lines.Insert(index, line1);
                            lines.Insert(index + 1, line2);
                            lines.Insert(index + 2, line3);
                        }
                        else
                        {
                            lines.AddRange( new List<ICustomLine> (){ line1, line2, line3});
                        }
                    }
                    msPointFinder.SetLines(lines);
                    mspoints[i] = msPointFinder.FindMSPointForLine(line1);
                    mspoints[j] = msPointFinder.FindMSPointForLine(line2);
                    mspoints.Insert(j + 1, msPointFinder.FindMSPointForLine(line3));
                    i--;
                }
            }
            return mspoints;
        }

        private ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }
    }
}
