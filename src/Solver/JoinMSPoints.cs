using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace  MidSurfaceNameSpace.Solver
{
    public class JoinMSPoints : IJoinMSPoints
    {
        public JoinMSPoints() { }

        public IMidSurface Join(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            IMidSurface midsurface = new MidSurfaceNameSpace.MidSurface();
            /*var points = JoinPoints(mspointfinder, mspoints, accuracy);

            for (int i = 0; i < points.Count() - 1; i++)
            {
                midsurface.Add(PointsToSegment(points[i], points[i + 1]));
            }*/
            return midsurface;
        }

        private List<Point> JoinPoints(IMSPointFinder mspointfinder, List<IMSPoint> mspoints, double accuracy)
        {
            List<Point> result = new List<Point>();

            /*for (var i = 0; i < mspoints.Count() - 1; i++)
            {
                if (mspoints[i].GetDistance(mspoints[i + 1]) <= accuracy)
                {
                    result.Add(mspoints[i].GetMSPoint());
                }
                else
                {
                    var currentPointParents = mspoints[i].GetParents();
                    var nextPointParents = mspoints[i + 1].GetParents();

                    if (currentPointParents[0].GetN() == nextPointParents[1].GetN())
                    {
                        currentPointParents = new List<ICustomPoint>()
                            {
                                new CustomPoint(currentPointParents[0].GetN(), 
                                                Math.Abs(currentPointParents[1].GetT() - currentPointParents[0].GetT()) / 2, currentPointParents[0].GetAlpha()),
                                new CustomPoint(currentPointParents[0].GetN(),
                                                Math.Abs(nextPointParents[1].GetT() - nextPointParents[0].GetT()) / 2, currentPointParents[0].GetAlpha())
                            };
                    }
                    else
                    {  //TO DO: add bisector implementation
                        //var bisectorPoint = new CustomPoint(currentPointParents[1].GetN(),
                        //                        currentPointParents[1].GetT(),
                        //                        currentPointParents[0].GetAlpha() + nextPointParents[1].GetAlpha());

                        //result.Add(mspointfinder.FindBisectorPoint(bisectorPoint).GetMSPoint());

                        currentPointParents = new List<ICustomPoint>()
                            {
                                new CustomPoint(currentPointParents[0].GetN(),
                                                Math.Abs(currentPointParents[1].GetT() - currentPointParents[0].GetT()) / 2,
                                                currentPointParents[0].GetAlpha()),

                                nextPointParents[0],

                                new CustomPoint(currentPointParents[0].GetN(),
                                                Math.Abs(nextPointParents[1].GetT() - nextPointParents[0].GetT()) / 2,
                                                currentPointParents[0].GetAlpha())
                            };
                    }

                    var newPoints = mspointfinder.FindMSPoints(currentPointParents);
                    result.AddRange(JoinPoints(mspointfinder, newPoints, accuracy));
                }
            }*/
            return result;
        }

        ISegment PointsToSegment(Point begin, Point end)
        {
            /*return new Segment(new BezierCurve(), new List<Point> { begin, end });*/
            return null;
        }
    }
}
