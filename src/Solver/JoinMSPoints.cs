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
        internal class Clarification
        {
            private double accuracy;

            public Clarification(double accuracy)
            {
                this.accuracy = accuracy;
            }

            public bool Required(IMSPoint point1, IMSPoint point2)
            {
                var line1 = point1.GetLine();
                var line2 = point2.GetLine();

                // If mspoints belong to different contours
                if (line1.GetPoint2().GetPoint() != line2.GetPoint1().GetPoint())
                {
                    return false;
                }

                // If mspoints are suitable for accuracy 
                if ((point2.GetPoint() - point1.GetPoint()).Length <= accuracy)
                {
                    return false;
                }

                //If the difference between the lengths of their lines is less accuracy/4
                if ((line1.GetPoint1().GetPoint() - line2.GetPoint2().GetPoint()).Length <= accuracy/4)
                {
                    return false;
                }

                var vector1 = line1.GetPoint1().GetPoint() - line1.GetPoint2().GetPoint();
                var vector2 = line2.GetPoint2().GetPoint() - line2.GetPoint1().GetPoint();

                // If mspoints  segments are difference and form angle < 180
                if (Vector.AngleBetween(vector1, vector2) >= 0 &&
                    line1.GetPoint1().GetN() != line2.GetPoint2().GetN())
                {
                    return false;
                }
                return true;
            }
        }

        private List<ISegment> segments;
        private IMSPointFinder msPointFinder;
        private Clarification clarification;

        public JoinMSPoints(IMSPointFinder finder, List<ISegment> segments, double accuracy)
        {
            this.segments = segments;
            this.clarification = new Clarification(accuracy);
            this.msPointFinder = finder;
        }

        public IMidSurface Join()
        {
            IMidSurface midsurface = new MidSurface();
            var mspoints = Qlarify(msPointFinder.FindMSPoints());
            for (int i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;
                midsurface.Add(PointsToSegment(mspoints[i].GetPoint(), mspoints[j].GetPoint()));
            }
            return midsurface;
        }

        List<IMSPoint> Qlarify(List<IMSPoint> mspoints)
        {
            var lines = new List<ICustomLine>();

            foreach (var point in mspoints)
            {
                lines.Add(point.GetLine());
            }
            for (int i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;

                if (clarification.Required(mspoints[i], mspoints[j]))
                {
                    var currentLine = mspoints[i].GetLine();
                    var nextLine = mspoints[j].GetLine();

                    ICustomPoint point1 = null, point2 = null;

                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        var t1 = (currentLine.GetPoint2().GetT() + currentLine.GetPoint1().GetT()) / 2;
                        var t2 = (nextLine.GetPoint2().GetT() + nextLine.GetPoint1().GetT()) / 2;

                        point1 = new CustomPoint(currentLine.GetPoint1().GetN(), t1,
                            segments[currentLine.GetPoint1().GetN()].GetCurvePoint(t1));
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
                            lines.AddRange(new List<ICustomLine>() { line1, line2, line3 });
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
