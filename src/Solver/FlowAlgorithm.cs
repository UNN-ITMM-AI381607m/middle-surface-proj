using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{

    public class FlowAlgorithm : IAlgorithm
    {
        double splitterAccuracy;
        double detalizerAccuracy;
        public FlowAlgorithm(double splitterAccuracy, double detalizerAccuracy)
        {
            this.splitterAccuracy = splitterAccuracy;
            this.detalizerAccuracy = detalizerAccuracy;
        }

        public IMidSurface Run(ISolverData solverdata)
        {
            List<ICustomLine> simplifiedModel;
            IMidSurface midsurface = new MidSurface();
            List<IContour> contours = solverdata.GetContours();

            List<ISegment> segments = new List<ISegment>();
            foreach (var contour in contours)
            {
                segments.AddRange(contour.GetSegments());
            }

            double X_Min = segments[0].GetCurvePoint(0).X, X_Max = segments[0].GetCurvePoint(0).X, Y_Min = segments[0].GetCurvePoint(0).Y, Y_Max = segments[0].GetCurvePoint(0).Y;
            foreach (var seg in segments)
            {
                for (double t = 0d; t <= 1; t += 0.1)
                {
                    if (X_Min >= seg.GetCurvePoint(t).X) X_Min = seg.GetCurvePoint(t).X;
                    if (Y_Min >= seg.GetCurvePoint(t).Y) Y_Min = seg.GetCurvePoint(t).Y;
                    if (X_Max <= seg.GetCurvePoint(t).X) X_Max = seg.GetCurvePoint(t).X;
                    if (Y_Max <= seg.GetCurvePoint(t).Y) Y_Max = seg.GetCurvePoint(t).Y;
                }
            }

            simplifiedModel = new Splitter().Split(solverdata.GetContours(), splitterAccuracy);

            List<Point> mspoints = new List<Point>();
            Point prev = new Point();
            for (double d = X_Min + detalizerAccuracy / 10; d <= X_Max - detalizerAccuracy / 10; d += detalizerAccuracy)
            {
                List<Point> points = GetIntersectionPoints(simplifiedModel, new Point(d, Y_Min - detalizerAccuracy), new Point(d, Y_Max + detalizerAccuracy));

                points.Sort(new ForY());
                Point center;
                int index = 0;
                if (points.Count == 0) continue;
                else if (points.Count == 2)
                {
                    //// midsurface.Add(new Segment(new BezierCurve(), new List<Point> { points[0], points[1] })); //DEBUG
                    index = 0;
                    center = new Point(d, (points[index].Y + points[index + 1].Y) / 2);
                }
                //TODO: Dinar: avoid paths with intersections

                //else if (points.Count % 2 == 0)
                //{
                //    //midsurface.Add(new Segment(new BezierCurve(), new List<Point> { points[0], points[1] })); //DEBUG
                //    double MinDistance = Math.Max(X_Max - X_Min, Y_Max - Y_Min);

                //    for (int k = 0; k < points.Count - 1; k += 2)
                //    {

                //        List<Point> not_allowed = GetIntersectionPoints(simplifiedModel, prev, new Point(d, (points[k + 1].Y + points[k].Y) / 2));
                //        if (not_allowed.Count > 0) { continue; }
                //        if (Distance(prev, points[k], points[k + 1]) < MinDistance)
                //        {

                //            MinDistance = Distance(prev, points[k], points[k + 1]);
                //            index = k;
                //        }
                //    }

                //    center = new Point(d, (points[index + 1].Y + points[index].Y) / 2);
                //}
                //else if (points.Count > 0) { center = points[0]; }
                else
                    continue;

                ////TODO: Dinar: shift center for maximize R 
                //int step_line =5;
                //double diff = Math.Abs(points[index+1].Y - points[index].Y) / (double)(step_line + 1);
                //double prevR = Math.Abs(points[index + 1].Y - points[index].Y);
                //while (step_line-- >= -1)
                //{
                //    double R = Math.Abs(points[index + 1].Y - points[index].Y);
                //    int step = 5;
                //    while (step-- > -1)
                //    {
                //        if (GetIntersecCount(simplifiedModel, center, R) > 0)
                //            R = R * 0.5;
                //        else
                //            R = R * 1.5;
                //    }
                //    if (Math.Abs(prevR - R) < 0.1)
                //    {
                //        int p1 = GetIntersecCount(simplifiedModel, new Point(d, Math.Abs(points[index + 1].Y - points[index].Y) / 5 + points[index].Y), R);
                //        int p2= GetIntersecCount(simplifiedModel, new Point(d, 4 * Math.Abs(points[index + 1].Y - points[index].Y) / 5 + points[index].Y), R);
                //        if (p1==p2) { step_line = -1; }
                //        else if (p1>p2)
                //            center.Y = 4 * Math.Abs(points[index + 1].Y - points[index].Y) / 5 + points[index].Y;

                //        else center.Y = Math.Abs(points[index + 1].Y - points[index].Y) / 5 + points[index].Y;
                //    }
                //    else
                //    if (prevR > R)
                //    {
                //        diff = -diff;
                //        center.Y += diff;
                //    }
                //    else center.Y += diff;
                //    prevR = R;
                //}
                prev = center;
                mspoints.Add(center);


            }

            for (int i = 0; i < mspoints.Count - 1; ++i)
            {
                midsurface.Add(new Segment(new BezierCurve(), new List<Point> { mspoints[i], mspoints[i + 1] }));
            }

            return midsurface;

        }

        private class ForY : Comparer<Point>
        {
            public override int Compare(Point x, Point y)
            {
                return (int)(x.Y - y.Y);
            }
        }

        private double Distance(Point p, Point prev1, Point prev2)
        {
            return Math.Sqrt((Math.Pow((prev1.X + prev2.X) / 2 - p.X, 2) + (Math.Pow((prev2.Y + prev1.Y) / 2 - p.Y, 2))));
        }

        public int GetIntersecCount(List<ICustomLine> simplifiedModel, Point center, double R)
        {
            int res = 0;

            foreach (var line in simplifiedModel)
            {
                if ((Math.Pow(line.GetPoint1().GetPoint().X - center.X, 2) + Math.Pow(line.GetPoint1().GetPoint().Y - center.Y, 2) <= R * R) ||
                    (Math.Pow(line.GetPoint2().GetPoint().X - center.X, 2) + Math.Pow(line.GetPoint2().GetPoint().Y - center.Y, 2) <= R * R) ||
                    (Math.Pow((line.GetPoint1().GetPoint().X + line.GetPoint2().GetPoint().X) / 2 - center.X, 2) + Math.Pow((line.GetPoint1().GetPoint().Y + line.GetPoint2().GetPoint().Y) / 2 - center.Y, 2) <= R * R)) ++res;
            }

            return res;
        }

        public Tuple<double, double, double> LineEquation(Point p1, Point p2)
        {
            double A = p2.Y - p1.Y;
            double B = p1.X - p2.X;
            double C = -p1.X * (p2.Y - p1.Y) + p1.Y * (p2.X - p1.X);
            return new Tuple<double, double, double>(A, B, C);
        }
        private double vector_mult(double ax, double ay, double bx, double by) //векторное произведение
        {
            return ax * by - bx * ay;
        }
        public bool areCrossing(Point p1, Point p2, Point p3, Point p4)//проверка пересечения
        {
            double v1 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y);
            double v2 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y);
            double v3 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y);
            double v4 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y);
            if ((v1 * v2) < 0 && (v3 * v4) < 0)
                return true;
            return false;
        }

        Point CrossingPoint(Tuple<double, double, double> t1, Tuple<double, double, double> t2)
        {

            double a1 = t1.Item1; double b1 = t1.Item2; double c1 = t1.Item3; double a2 = t2.Item1; double b2 = t2.Item2; double c2 = t2.Item3;
            Point pt = new Point();
            double d = (double)(a1 * b2 - b1 * a2);
            double dx = (double)(-c1 * b2 + b1 * c2);
            double dy = (double)(-a1 * c2 + c1 * a2);
            pt.X = (double)(dx / d);
            pt.Y = (double)(dy / d);
            return pt;
        }

        private List<Point> GetIntersectionPoints(List<ICustomLine> simplifiedModel, Point x1, Point x2)
        {
            List<Point> points = new List<Point>();

            foreach (var line in simplifiedModel)
            {
                if (areCrossing(line.GetPoint1().GetPoint(), line.GetPoint2().GetPoint(), x1, x2))
                {
                    points.Add(CrossingPoint(LineEquation(line.GetPoint1().GetPoint(), line.GetPoint2().GetPoint()), LineEquation(x1, x2)));
                }
            }

            return points;
        }
    }
}