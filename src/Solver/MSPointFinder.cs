using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public partial class MSPointFinder : IMSPointFinder
    {
        List<ICustomLine> simplifiedModel;
        List<ISegment> segments;
        double Rmax;

        public MSPointFinder(List<ISegment> segments)
        {
            this.segments = segments;
            Rmax = getRmax();
        }

        public List<IMSPoint> FindMSPoints(List<ICustomLine> simplifiedModel)
        {
            this.simplifiedModel = simplifiedModel;
            List<IMSPoint> mspoints = new List<IMSPoint>();

            for (int i = 0; i < simplifiedModel.Count(); i++)
            {
                ICustomPoint point1 = simplifiedModel[i].GetPoint1();
                ICustomPoint point2 = simplifiedModel[i].GetPoint2();
                double X = (point2.GetPoint().X + point1.GetPoint().X) / 2;
                double Y = (point2.GetPoint().Y + point1.GetPoint().Y) / 2;

                if (point2.GetPoint().X == point1.GetPoint().X && point2.GetPoint().Y == point1.GetPoint().Y)
                {
                    X = (segments[point2.GetN()].GetCurvePoint(point2.GetT()).X + segments[point1.GetN()].GetCurvePoint(point1.GetT()).X) / 2;
                    Y = (segments[point2.GetN()].GetCurvePoint(point2.GetT()).Y + segments[point1.GetN()].GetCurvePoint(point1.GetT()).Y) / 2;
                }
                
                mspoints.Add(GetMSPoint(simplifiedModel[i].GetRightNormal(), new Point(X, Y), simplifiedModel[i]));
            }
            return mspoints;
        }

        int checkMutualArrangementLineCircle(Point line1, Point line2, Point center, double R)
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

        public IMSPoint GetMSPoint(Vector vector, Point point, ICustomLine line)
        {
            double Rmax = this.Rmax;
            double Rmin = 0;
            double R = Rmax;
            Point center = new Point(point.X + vector.X * R, point.Y + vector.Y * R);
            int crossStatus = ValidateCircleDueModel(center, R, point);

            while (!EqualDoubles(Rmax, Rmin, 0.001))
            {
                R = (Rmax + Rmin) / 2;

                center.X = point.X + vector.X * R;
                center.Y = point.Y + vector.Y * R;

                crossStatus = ValidateCircleDueModel(center, R, point);
                if (crossStatus == 1)
                {
                    Rmin = R;
                }
                else if (crossStatus == -1)
                {
                    Rmax = R;
                }
                else if (crossStatus == 0)
                {
                    break;
                }
            }

            return new MSPoint(center, line);
        }

        public List<ISegment> GetSegments()
        {
            return segments;
        }

        bool IsRivol(Point rivol, Point point)
        {
            if (EqualDoubles(rivol.X, point.X, 1) && EqualDoubles(rivol.Y, point.Y, 1))
                return true;
            return false;
        }

        int ValidateCircleDueModel(Point center, double R, Point rivol)
        {
            bool foundGood = false;
            bool needToDecrease = false;
            bool needToIncrease = false;
            for (int i =0; i < simplifiedModel.Count(); i++)
            {
                Point linePoint1 = simplifiedModel[i].GetPoint1().GetPoint();
                Point linePoint2 = simplifiedModel[i].GetPoint2().GetPoint();

                Point resultPoint1 = new Point();
                Point resultPoint2 = new Point();
                int intersecCounter = 0;
                int lineIntersecsFound = LineCircleIntersection(center, R, linePoint1, linePoint2, ref resultPoint1, ref resultPoint2);
                double xmin = Math.Min(linePoint1.X, linePoint2.X) - 0.01;
                double xmax = Math.Max(linePoint1.X, linePoint2.X) + 0.01;
                double ymin = Math.Min(linePoint1.Y, linePoint2.Y) - 0.01;
                double ymax = Math.Max(linePoint1.Y, linePoint2.Y) + 0.01;
                if (lineIntersecsFound >= 1
                    && resultPoint1.X >= xmin && resultPoint1.X <= xmax
                    && resultPoint1.Y >= ymin && resultPoint1.Y <= ymax)
                {
                    if (!IsRivol(rivol, resultPoint1))
                        intersecCounter++;
                }
                if (lineIntersecsFound == 2
                    && resultPoint2.X >= xmin && resultPoint2.X <= xmax
                    && resultPoint2.Y >= ymin && resultPoint2.Y <= ymax)
                {
                    if (!IsRivol(rivol, resultPoint2))
                        intersecCounter++;
                }
                int mutualArrangement = checkMutualArrangementLineCircle(linePoint1, linePoint2, center, R);
                if (intersecCounter == 1 && mutualArrangement == 0)
                    foundGood = true;
                else if (intersecCounter == 0 && mutualArrangement == 0)
                    needToIncrease = true;
                else
                    needToDecrease = true;
            }
            if (needToDecrease)
                return -1;
            else if (needToIncrease)
                return 1;
            else if (foundGood)
                return 0;
            //Can not be
            return -2;
        }
        bool EqualDoubles(double n1, double n2, double precision_)
        {
            return (Math.Abs(n1 - n2) <= precision_);
        }

        int LineCircleIntersection(Point center, double R, // центр и рдиус окружности
                           Point point1,           // точки
                           Point point2,           //    отрезка
                           ref Point resultPoint1,         // резуль-
                           ref Point resultPoint2)         //      тат
        {
            double q = center.X * center.X + center.Y * center.Y - R * R;
            double k = -2.0 * center.X;
            double l = 2.0 * center.Y;

            double z = point1.X * point2.Y - point2.X * point1.Y;
            double p = point1.Y - point2.Y;
            double s = point2.X - point1.X;

            if (EqualDoubles(s, 0.0, 0.001))
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


        double getRmax()
        {
            double R, Xmax = 0, Xmin = double.MaxValue, Ymax = 0, Ymin = double.MaxValue;
            for (int i = 0; i < segments.Count; i++)
            {
                for (double j = 0.0; j < 1.0; j += 0.1)
                {
                    double X = segments[i].GetCurvePoint(j).X;
                    double Y = segments[i].GetCurvePoint(j).Y;
                    if (X < Xmin)
                        Xmin = X;
                    else if (X > Xmax)
                        Xmax = X;
                    if (Y < Ymin)
                        Ymin = Y;
                    else if (Y > Ymax)
                        Ymax = Y;
                }
            }
            R = Math.Max(Xmax - Xmin, Ymax - Ymin) / 2;
            return R;
        }
    }
}
