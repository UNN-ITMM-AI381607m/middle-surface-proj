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

        public MSPointFinder()
        {
        }

        public void SetLines(List<ICustomLine> lines)
        {
            simplifiedModel = lines;
            Rmax = CalculateMaxRadius();
        }

        public List<IMSPoint> FindMSPoints()
        {
            List<IMSPoint> mspoints = new List<IMSPoint>();

            foreach (var line in simplifiedModel)
            {
                Point middlePoint = new Point((line.GetPoint1().GetPoint().X + line.GetPoint2().GetPoint().X) / 2,
                (line.GetPoint1().GetPoint().Y + line.GetPoint2().GetPoint().Y) / 2);
                mspoints.Add(FindMSPoint(middlePoint, line.GetRightNormal()));
            }
            return mspoints;
        }

        IMSPoint CalculateMSPoint(Vector vector, Point point, ICustomLine line)
        {
            double Rmax = this.Rmax;
            double Rmin = 0;
            double R = Rmax;
            Point center = new Point(point.X + vector.X * R, point.Y + vector.Y * R);
            int crossStatus = ValidateCircleDueModel(center, R, point);

            while (!Algorithm.EqualDoubles(Rmax, Rmin, 0.0001))
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
            //Hack
            center.X = point.X + vector.X * Rmax;
            center.Y = point.Y + vector.Y * Rmax;
            return new MSPoint(center, line);
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

                // if line is point (bisector case)
                if (linePoint1 == linePoint2) continue;

                //if mspoint from bisector
                //bool mspointFromBisector = currentLine.GetPoint1().GetPoint() == currentLine.GetPoint2().GetPoint();
                //if (mspointFromBisector &&
                //    (currentLine.GetPoint1().GetPoint() == linePoint1 ||
                //    currentLine.GetPoint1().GetPoint() == linePoint2))
                //    continue;

                //if (currentLine.GetPoint1().GetPoint() == linePoint1 &&
                //    currentLine.GetPoint2().GetPoint() == linePoint2)
                //    continue;

                Point resultPoint1 = new Point();
                Point resultPoint2 = new Point();
                int intersecCounter = CustomLine.LineSegmentIntersectionCircle(center, R, linePoint1, linePoint2, ref resultPoint1, ref resultPoint2);
                int mutualArrangement = CustomLine.CheckMutualArrangementLineCircle(linePoint1, linePoint2, center, R);
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

        public IMSPoint FindMSPoint(Point contourPoint, Vector guidingVector)
        {
            return CalculateMSPoint(guidingVector, contourPoint, null);
        }

        public IMSPoint FindMSPointForLine(ICustomLine line, Vector guidingVector = default(Vector))
        {
            Point middlePoint = new Point((line.GetPoint1().GetPoint().X + line.GetPoint2().GetPoint().X) / 2,
                (line.GetPoint1().GetPoint().Y + line.GetPoint2().GetPoint().Y) / 2);
            Vector guiding = guidingVector.Length == 0 ? line.GetRightNormal() : guidingVector;
            return CalculateMSPoint(guiding, middlePoint, line);
        }
    }
}
