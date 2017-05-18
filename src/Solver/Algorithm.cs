using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class Algorithm : IAlgorithm
    {
        double splitterAccuracy;
        double detalizerAccuracy;
        double maxLengthModel;
        const int maxCycleSize = 20;

        public Algorithm(double splitterAccuracy, double detalizerAccuracy)
        {
            this.splitterAccuracy = splitterAccuracy;
            this.detalizerAccuracy = detalizerAccuracy;
        }

        private class BaseAlgorithm
        {
            List<ICustomLine> simplifiedModel;
            public BaseAlgorithm()
            {
                simplifiedModel = new List<ICustomLine>();
            }

            public IEnumerable<ICustomLine> GetSimplifiedModel()
            {
                return simplifiedModel;
            }

            public List<IMSPoint> Run(ISolverData solverdata, double splitterAccuracy, double detalizerAccuracy)
            {
                List<IContour> contours = solverdata.GetContours();

                List<ISegment> segments = new List<ISegment>();
                foreach (var contour in contours)
                {
                    segments.AddRange(contour.GetSegments());
                }

                simplifiedModel = new Splitter().Split(solverdata.GetContours(), splitterAccuracy);
                IMSPointFinder mspointfinder = new MSPointFinder(simplifiedModel);

                IDetailizer detailizer = new Detailizer(mspointfinder, simplifiedModel, segments, detalizerAccuracy);
                return detailizer.Detalize();
            }
        }

        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();
            maxLengthModel = FindMaxLength(solverdata.GetContours(), 0.01);

            BaseAlgorithm baseAlgorithm = new BaseAlgorithm();
            List<IMSPoint> msPoints = baseAlgorithm.Run(solverdata, splitterAccuracy * maxLengthModel, detalizerAccuracy);

            //Graph msGraph = ConstructGraph(msPoints, baseAlgorithm.GetSimplifiedModel());
            //msGraph.RemoveCycles(maxCycleSize);

            //List<Point> points = msGraph.GetPath();

            ////Точки для работы
            //List<IMSPoint> new_mspoints = ConvertPointToMSPoint(points, msPoints);

            IJoinMSPoints jointpoints = new JoinMSPoints(null);

            return jointpoints.Join(msPoints);
        }

        List<IMSPoint> ConvertPointToMSPoint(List<Point> points, List<IMSPoint> mspoints)
        {
            List<IMSPoint> new_mspoints = new List<IMSPoint>();

            foreach (var point in points)
            {
                foreach (var mspoint in mspoints)
                {
                    if ((point.X == mspoint.GetPoint().X) && (point.Y == mspoint.GetPoint().Y))
                        new_mspoints.Add(mspoint);
                }
            }

            return new_mspoints;
        }

        Graph ConstructGraph(IEnumerable<IMSPoint> msPoints, IEnumerable<ICustomLine> simplifiedModel)
        {
            Graph graph = new Graph();

            List<int> connectionOrder = new List<int>();
            var marks = SetMarks(msPoints, simplifiedModel);
            foreach (var mark in marks)
            {
                connectionOrder.Add(mark.MSPointIndex);
            }

            for (int i = 0; i < connectionOrder.Count - 1; i++)
            {
                graph.AddEdge(msPoints.ElementAt(connectionOrder[i]).GetPoint(), msPoints.ElementAt(connectionOrder[i + 1]).GetPoint());
            }

            //for (int i = 0; i < msPoints.Count(); i++)
            //{
            //    int j = i + 1 == msPoints.Count() ? 0 : i + 1;
            //    graph.AddEdge(msPoints.ElementAt(i).GetPoint(), msPoints.ElementAt(j).GetPoint());
            //}

            return graph;
        }

        //Temporary
        List<Mark> SetMarks(IEnumerable<IMSPoint> mspoints, IEnumerable<ICustomLine> simplifiedModel)
        {
            List<Mark> marks = new List<Mark>();
            int idCounter = 0;
            foreach (var mspoint in mspoints)
            {
                foreach (var line in simplifiedModel)
                {
                    Point intersecPoint1 = new Point();
                    Point intersecPoint2 = new Point();
                    int intersecCount = CustomLine.LineSegmentIntersectionCircle(mspoint.GetPoint(), mspoint.GetRadius(),
                        line.GetPoint1().GetPoint(),
                        line.GetPoint2().GetPoint(), ref intersecPoint1, ref intersecPoint2);
                    if (intersecCount == 1)
                    {
                        AddMark(idCounter, intersecPoint1, line.GetPoint1(), ref marks);
                    }
                    else if (intersecCount == 2)
                    {
                        Point contactPoint = Vector.Add((intersecPoint2 - intersecPoint1) / 2, intersecPoint1);
                        AddMark(idCounter, contactPoint, line.GetPoint1(), ref marks);
                    }
                }
                idCounter++;
            }
            return marks;
        }

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

        public void AddMark(int id, Point contactPoint, ICustomPoint point1, ref List<Mark> marks)
        {
            if (marks.Any(x => (x.ContactPoint - contactPoint).Length <= 0.1))
                return;

            Mark newMark = new Mark(id, contactPoint);
            int index = marks.FindIndex(x => (x.ContactPoint - point1.GetPoint()).Length > (contactPoint - point1.GetPoint()).Length);
            if (index == -1)
                marks.Add(newMark);
            else
                marks.Insert(index, newMark);
        }

        public static bool EqualDoubles(double n1, double n2, double precision_)
        {
            return (Math.Abs(n1 - n2) <= precision_);
        }

        public static double FindMaxLength(IEnumerable<IContour> contours, double step)
        {
            double maxLength = 0;
            foreach (var contour in contours)
            {
                IEnumerable<ISegment> segments = contour.GetSegments();
                foreach (var segment in segments)
                {
                    double length = 0;
                    double t = 0;
                    while (t < 1)
                    {
                        double nextT = t + step;
                        if (nextT > 1)
                        {
                            nextT = 1;
                        }
                        Point currentPoint = segment.GetCurvePoint(t);
                        Point nextPoint = segment.GetCurvePoint(nextT);
                        length += (currentPoint - nextPoint).Length;

                        t = nextT;
                    }
                    if (maxLength < length)
                    {
                        maxLength = length;
                    }
                }
            }

            return maxLength;
        }

    }
}
