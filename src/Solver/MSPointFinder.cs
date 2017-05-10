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
        double Rmax;
        const double radiusAccuracy = 0.00001;
        const double closePointsAccuracy = 5;
        int MSPointCounter;


        public MSPointFinder(List<ICustomLine> lines)
        {
            MSPointCounter = 0;
            simplifiedModel = lines;
            Rmax = CalculateMaxRadius();
        }

        void SetMarks(Point center, double R, int id)
        {
            for (int i = 0; i < simplifiedModel.Count; i++)
            {
                Point intersecPoint1 = new Point();
                Point intersecPoint2 = new Point();
                int intersecCount = CustomLine.LineSegmentIntersectionCircle(center, R, simplifiedModel[i].GetPoint1().GetPoint(),
                    simplifiedModel[i].GetPoint2().GetPoint(), ref intersecPoint1, ref intersecPoint2);
                if (intersecCount == 1)
                {
                    simplifiedModel[i].AddMark(id, intersecPoint1);
                }
                else if (intersecCount == 2)
                {
                    Point contactPoint = Vector.Add((intersecPoint2 - intersecPoint1) / 2, intersecPoint1);
                    simplifiedModel[i].AddMark(id, contactPoint);
                }
            }
        }

        IMSPoint CalculateMSPoint(Point point, Normal normal)
        {
            var vector = new Vector(normal.Dx(), normal.Dy());
            var segment = normal.Segment();

            double Rmax = this.Rmax;
            double Rmin = 0;
            double R = Rmax;
            Point center = new Point(point.X + vector.X * R, point.Y + vector.Y * R);
            int crossStatus = ValidateCircleDueModel(center, R, point);
            double RMaxPrevious = Rmax;
            while (!Algorithm.EqualDoubles(Rmax, Rmin, radiusAccuracy))
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
                    RMaxPrevious = Rmax;
                    Rmax = R;
                }
                else if (crossStatus == 0)
                {
                    break;
                }
            }
            SetMarks(center, RMaxPrevious, MSPointCounter);
            MSPointCounter++;
            return new MSPoint(center, segment);
        }

        bool ClosePoints(Point a, Point b, double accuracy)
        {
            if ((a - b).Length <= accuracy)
                return true;
            return false;
        }

        int ValidateCircleDueModel(Point center, double R, Point currentPoint)
        {
            bool foundGood = false;
            bool needToDecrease = false;
            bool needToIncrease = false;
            for (int i = 0; i < simplifiedModel.Count(); i++)
            {
                Point linePoint1 = simplifiedModel[i].GetPoint1().GetPoint();
                Point linePoint2 = simplifiedModel[i].GetPoint2().GetPoint();

                Point resultPoint1 = new Point();
                Point resultPoint2 = new Point();
                int intersecCounter = CustomLine.LineSegmentIntersectionCircle(center, R, linePoint1, linePoint2, ref resultPoint1, ref resultPoint2);

                if ((intersecCounter == 1 && ClosePoints(resultPoint1, currentPoint, closePointsAccuracy)) ||
                    (intersecCounter == 2 && ClosePoints(Vector.Add((resultPoint1 - resultPoint2) / 2, resultPoint2), currentPoint, closePointsAccuracy)))
                {
                    continue;
                }

                int mutualArrangement = CustomLine.CheckMutualArrangementLineCircle(linePoint1, linePoint2, center, R, 0.001);
                if (intersecCounter == 1 && mutualArrangement == 0)
                {
                    foundGood = true;
                }
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
            return CalculateMSPoint(contourPoint, normal);
        }
    }
}
