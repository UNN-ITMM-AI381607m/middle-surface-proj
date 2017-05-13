using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class MSPointFinder : IMSPointFinder
    {
        List<ICustomLine> simplifiedModel;
        double Rmax;
        const double radiusAccuracy = 0.00001;
        const double closePointsAccuracy = 5;

        public MSPointFinder(List<ICustomLine> lines)
        {
            simplifiedModel = lines;
            Rmax = CalculateMaxRadius();
        }

        bool ClosePoints(Point a, Point b, double accuracy)
        {
            if ((a - b).Length <= accuracy)
                return true;
            return false;
        }

        ICustomLine FindLineWithPoint(Point point)
        {
            foreach (var line in simplifiedModel)
            {
                if (CustomLine.IsPointBelongsToLine(line.GetPoint1().GetPoint(),
                    line.GetPoint2().GetPoint(), point, 0.01))
                    return line;      
            }
            return null;
        }

        int ValidateCircleDueModel(Point center, double R, Point currentPoint, ICustomLine currentLine)
        {
            foreach (var line in simplifiedModel)
            {
                Point linePoint1 = line.GetPoint1().GetPoint();
                Point linePoint2 = line.GetPoint2().GetPoint();

                Point resultPoint1 = new Point();
                Point resultPoint2 = new Point();

                //if (currentLine != null && Math.Abs(Vector.AngleBetween(currentLine.GetRightNormal(), line.GetRightNormal())) < 40)
                //    continue;

                int intersecCounter = CustomLine.LineSegmentIntersectionCircle(center, R, linePoint1, linePoint2, ref resultPoint1, ref resultPoint2);
                if (intersecCounter < 2
                    && CustomLine.CheckMutualArrangementLineCircle(linePoint1, linePoint2, center, R, 0.001) == 0)
                    continue;
                if (!ClosePoints(Vector.Add((resultPoint1 - resultPoint2) / 2, resultPoint2), currentPoint, closePointsAccuracy))
                    return -1;

            }

            return 1;
        }

        double CalculateMaxRadius()
        {
            double Xmax = 0;
            double Xmin = double.MaxValue;
            double Ymax = 0;
            double Ymin = double.MaxValue;

            for (int i = 0; i < simplifiedModel.Count; i++)
            {
                List<Point> linePoints = new List<Point>()
                {
                    simplifiedModel[i].GetPoint1().GetPoint(),
                    simplifiedModel[i].GetPoint2().GetPoint()
                };

                foreach (var point in linePoints)
                {
                    if (point.X < Xmin)
                        Xmin = point.X;
                    else if (point.X > Xmax)
                        Xmax = point.X;
                    if (point.Y < Ymin)
                        Ymin = point.Y;
                    else if (point.Y > Ymax)
                        Ymax = point.Y;
                }
            }
            return Math.Max(Xmax - Xmin, Ymax - Ymin) / 2;
        }

        public IMSPoint FindMSPoint(Point contourPoint, Normal normal)
        {
            var vector = new Vector(normal.Dx(), normal.Dy());
            var segment = normal.Segment();

            var currentLine = FindLineWithPoint(contourPoint);

            double Rmax = this.Rmax;
            double Rmin = 0;
            double R = Rmax;
            Point center = new Point(contourPoint.X + vector.X * R, contourPoint.Y + vector.Y * R);
            int crossStatus = ValidateCircleDueModel(center, R, contourPoint, currentLine);
            double RMaxPrevious = Rmax;
            while (!Algorithm.EqualDoubles(Rmax, Rmin, radiusAccuracy))
            {
                R = (Rmax + Rmin) / 2;

                center.X = contourPoint.X + vector.X * R;
                center.Y = contourPoint.Y + vector.Y * R;

                crossStatus = ValidateCircleDueModel(center, R, contourPoint, currentLine);
                if (crossStatus == 1)
                {
                    Rmin = R;
                }
                else if (crossStatus == -1)
                {
                    RMaxPrevious = Rmax;
                    Rmax = R;
                }
                else if (crossStatus == 0)
                {
                    break;
                }
            }
            return new MSPoint(center, segment, RMaxPrevious);
        }
    }
}
