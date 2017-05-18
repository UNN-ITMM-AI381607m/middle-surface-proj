using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
	
    public class CustomLine: ICustomLine
    {
        ICustomPoint point1;
        ICustomPoint point2;
        Vector rightNormalVector;

        public CustomLine(ICustomPoint firstPoint, ICustomPoint secondPoint)
        {
            point1 = firstPoint;
            point2 = secondPoint;
            CalculateNormalVector();
        }

        public ICustomPoint GetPoint1()
        {
            return point1;
        }

        public ICustomPoint GetPoint2()
        {
            return point2;
        }

        public Vector GetRightNormal()
        {
            return rightNormalVector;
        }

        void CalculateNormalVector()
        {
            double dx = point2.GetPoint().X - point1.GetPoint().X;
            double dy = point2.GetPoint().Y - point1.GetPoint().Y;
            rightNormalVector = new Vector(dy, -dx);
            rightNormalVector.Normalize();
        }

        public static int CheckMutualArrangementLineCircle(Point line1, Point line2, Point center, double R, double accuracy)
        {
            double RMax = R + accuracy;
            double RMin = R - accuracy;
            double d1 = new Vector(line1.X - center.X, line1.Y - center.Y).Length;
            double d2 = new Vector(line2.X - center.X, line2.Y - center.Y).Length;
            if (d1 > RMin && d2 > RMin)
                return 0;
            if ((d1 >= RMin && d2 <= RMax) || (d1 <= RMax && d2 >= RMin))
                return 1;
            if (d1 <= RMax && d2 <= RMax)
                return 2;
            return -1;
        }

        public static int LineSegmentIntersectionCircle(Point center, double R, Point lineSegmentPoint1, Point lineSegmentPoint2,
            ref Point intersecPoint1, ref Point intersecPoint2)
        {
            Point resultPoint1 = new Point();
            Point resultPoint2 = new Point();
            int intersecCounter = 0;
            int lineIntersecsFound = LineCircleIntersection(center, R, lineSegmentPoint1, lineSegmentPoint2, ref resultPoint1, ref resultPoint2);

            if (lineIntersecsFound >= 1
                && IsPointBelongsToLine(lineSegmentPoint1, lineSegmentPoint2, intersecPoint1, 0.1))
            {
                intersecCounter++;
                intersecPoint1 = resultPoint1;
            }
            if (lineIntersecsFound == 2
                && IsPointBelongsToLine(lineSegmentPoint1, lineSegmentPoint2, intersecPoint2, 0.1))
            {
                intersecCounter++;
                if (intersecCounter == 1)
                {
                    intersecPoint2 = resultPoint1;
                }
                else
                    intersecPoint2 = resultPoint2;
            }
            return intersecCounter;
        }

        public static bool IsPointBelongsToLine(Point linePoint1, Point linePoint2, Point point, double lineAccuracy)
        {
            double xmin = Math.Min(linePoint1.X, linePoint2.X) - lineAccuracy;
            double xmax = Math.Max(linePoint1.X, linePoint2.X) + lineAccuracy;
            double ymin = Math.Min(linePoint1.Y, linePoint2.Y) - lineAccuracy;
            double ymax = Math.Max(linePoint1.Y, linePoint2.Y) + lineAccuracy;
            if (point.X >= xmin && point.X <= xmax
                && point.Y >= ymin && point.Y <= ymax)
                return true;
            return false;
        }

        public static int LineCircleIntersection(Point center, double R,
                           Point point1,
                           Point point2,
                           ref Point resultPoint1,
                           ref Point resultPoint2)
        {
            double q = center.X * center.X + center.Y * center.Y - R * R;
            double k = -2.0 * center.X;
            double l = 2.0 * center.Y;

            double z = point1.X * point2.Y - point2.X * point1.Y;
            double p = point1.Y - point2.Y;
            double s = point2.X - point1.X;

            if (Algorithm.EqualDoubles(s, 0.0, 0.001))
            {
                s = 0.001;
            }

            double A = s * s + p * p;
            double B = s * s * k + 2.0 * z * p + s * l * p;
            double C = q * s * s + z * z + s * l * z;

            double D = B * B - 4.0 * A * C;

            if (D < 0.0)
            {
                return 0;
            }
            else if (D < 0.005)
            {
                resultPoint1.X = -B / (2.0 * A);
                resultPoint1.Y = -(p * resultPoint1.X + z) / s;
                return 1;
            }
            else
            {
                resultPoint1.X = (-B + Math.Sqrt(D)) / (2.0 * A);
                resultPoint1.Y = -(p * resultPoint1.X + z) / s;

                resultPoint2.X = (-B - Math.Sqrt(D)) / (2.0 * A);
                resultPoint2.Y = -(p * resultPoint2.X + z) / s;
            }

            return 2;
        }

        public static bool CheckLinesIntersection(Point p1, Point p2, Point p3, Point p4)
        {
            double k1 = (p2.Y - p1.Y) / (p2.X - p1.X);
            double b1 = p1.Y - k1 * p1.X;

            double k2 = (p4.Y - p3.Y) / (p4.X - p3.X);
            double b2 = p3.Y - k2 * p3.X;

            double x = (b1 - b2) / (k2 - k1);
            double y = k1 * x + b1;

            double xmin = Math.Min(p1.X, p2.X);
            double xmax = Math.Max(p1.X, p2.X);
            double ymin = Math.Min(p1.Y, p2.Y);
            double ymax = Math.Max(p1.Y, p2.Y);

            double xmin1 = Math.Min(p3.X, p4.X);
            double xmax1 = Math.Max(p3.X, p4.X);
            double ymin1 = Math.Min(p3.Y, p4.Y);
            double ymax1 = Math.Max(p3.Y, p4.Y);

            if (x < xmax && x > xmin && y > ymin && y < ymax &&
                x < xmax1 && x > xmin1 && y > ymin1 && y < ymax1)
            {
                return true;
            }
            return false;
        }
    }
}
