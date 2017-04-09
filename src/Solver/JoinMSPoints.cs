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
            IMidSurface midsurface = new MidSurfaceNameSpace.MidSurface();
            var points = JoinPoints(mspointfinder, mspoints, accuracy);

            for (int i = 0; i < points.Count() - 1; i++)
            {
                midsurface.Add(PointsToSegment(points[i], points[i + 1]));
            }
            return midsurface;
        }

        private List<Point> JoinPoints(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            List<Point> result = new List<Point>();

            for (var i = 0; i < mspoints.Count() - 1; i++)
            {
                if ((mspoints[i + 1].GetPoint() - mspoints[i].GetPoint()).Length <= accuracy)
                {
                    result.Add(mspoints[i].GetPoint());
                }
                else
                {
                    var currentLine = mspoints[i].GetLine();
                    var nextLine = mspoints[i + 1].GetLine();

                    List<ICustomLine> lines = new List<ICustomLine>();
                    if (currentLine.GetPoint1().GetN() == nextLine.GetPoint2().GetN())
                    {
                        currentLine = new CustomLine
                            (
                                new CustomPoint(currentLine.GetPoint1().GetN(),
                                                Math.Abs(currentLine.GetPoint2().GetT() - currentLine.GetPoint1().GetT()) / 2 ),
                                new CustomPoint(nextLine.GetPoint1().GetN(),
                                                Math.Abs(nextLine.GetPoint2().GetT() - nextLine.GetPoint1().GetT()) / 2)
                            );

                        lines.Add(currentLine);
                    }
                    else
                    {  //TO DO: add bisector implementation
                       //var bisector = mspointfinder.FindBisectorPoint(currentLine, nextLine);

                        currentLine = new CustomLine
                            (
                                new CustomPoint(currentLine.GetPoint1().GetN(),
                                                Math.Abs(currentLine.GetPoint2().GetT() - currentLine.GetPoint1().GetT()) / 2),
                                currentLine.GetPoint2()
                            );

                         nextLine = new CustomLine
                            (
                                nextLine.GetPoint1(),
                                new CustomPoint(nextLine.GetPoint1().GetN(),
                                                Math.Abs(nextLine.GetPoint2().GetT() - nextLine.GetPoint1().GetT()) / 2)
                            );
           
                        lines.Add(currentLine);
                        lines.Add(nextLine);
                    }

                    var newPoints = mspointfinder.FindMSPoints(lines);
                    result.AddRange(JoinPoints(mspointfinder, newPoints, accuracy));
                }
            }
            result.Add(mspoints.Last().GetPoint());
            return result;
        }

        ISegment PointsToSegment(Point begin, Point end)
        {
            return new Segment(new BezierCurve(), new List<Point> { begin, end });
        }
    }
}
