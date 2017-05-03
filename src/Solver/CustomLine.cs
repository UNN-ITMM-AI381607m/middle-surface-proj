using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public struct Mark
    {
        public int MSPointIndex { get; private set; }
        public Point ContactPoint { get; private set; }
        public Mark(int index, Point point)
        {
            MSPointIndex = index;
            ContactPoint = point;
        }
    }
	
    public class CustomLine: ICustomLine
    {
        ICustomPoint point1;
        ICustomPoint point2;
        Vector rightNormalVector;

        const double markAccuracy = 1;

        List<Mark> marks;

        public CustomLine(ICustomPoint firstPoint, ICustomPoint secondPoint)
        {
            point1 = firstPoint;
            point2 = secondPoint;
            CalculateNormalVector();
            marks = new List<Mark>();
        }

        public IEnumerable<Mark> GetMarks()
        {
            return marks;
        }

        public void AddMark(int id, Point contactPoint)
        {
            if (marks.Any(x => (x.ContactPoint - contactPoint).Length <= markAccuracy))
                return;

            Mark newMark = new Mark(id, contactPoint);
            int index = marks.FindIndex(x => (x.ContactPoint - point1.GetPoint()).Length > (contactPoint - point1.GetPoint()).Length);
            if (index == -1)
                marks.Add(newMark);
            else
                marks.Insert(index, newMark);
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

        public static int CheckMutualArrangementLineCircle(Point line1, Point line2, Point center, double R)
        {
            double d1 = new Vector(line1.X - center.X, line1.Y - center.Y).Length;
            double d2 = new Vector(line2.X - center.X, line2.Y - center.Y).Length;
            if (d1 >= R && d2 >= R)
                return 0;
            if ((d1 >= R && d2 <= R) || (d1 <= R && d2 >= R))
                return 1;
            if (d1 < R && d2 < R)
                return 2;
            return -1;
        }

        public static int LineSegmentIntersectionCircle(Point center, double R, Point lineSegmentPoint1, Point lineSegmentPoint2,
            ref Point intersecPoint1, ref Point intersecPoint2)
        {
            int intersecCounter = 0;
            int lineIntersecsFound = LineCircleIntersection(center, R, lineSegmentPoint1, lineSegmentPoint2, ref intersecPoint1, ref intersecPoint2);

            if (lineIntersecsFound >= 1
                && IsPointBelongsToLine(lineSegmentPoint1, lineSegmentPoint2, intersecPoint1, 0.1))
            {
                intersecCounter++;
            }
            if (lineIntersecsFound == 2
                && IsPointBelongsToLine(lineSegmentPoint1, lineSegmentPoint2, intersecPoint2, 0.1))
            {
                intersecCounter++;
                if (intersecCounter == 1)
                    intersecPoint1 = intersecPoint2;
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
    }
}
