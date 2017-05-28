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
            if (splitterAccuracy < 0.001)
                splitterAccuracy = 0.001;
            else if (splitterAccuracy > 1)
                splitterAccuracy = 1;
            if (detalizerAccuracy < 0.001)
                detalizerAccuracy = 0.001;
            else if (detalizerAccuracy > 1)
                detalizerAccuracy = 1;
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
                IMSPointFinder mspointfinder = new MSPointFinder(simplifiedModel, splitterAccuracy);

                IDetailizer detailizer = new Detailizer(mspointfinder, simplifiedModel, segments, detalizerAccuracy);
                return detailizer.Detalize();
            }
        }

        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();
            maxLengthModel = FindMaxLength(solverdata.GetContours(), 0.01);

            BaseAlgorithm baseAlgorithm = new BaseAlgorithm();
            List<IMSPoint> msPoints = baseAlgorithm.Run(solverdata, splitterAccuracy * maxLengthModel,
                detalizerAccuracy * maxLengthModel);

            //Graph msGraph = ConstructGraph(msPoints);
            //msGraph.RemoveCycles(maxCycleSize);

            //List<Point> points = msGraph.GetPath();

            ////Точки для работы
            //List<IMSPoint> new_mspoints = ConvertPointToMSPoint(points, msPoints);

            IJoinMSPoints jointpoints = new JoinMSPoints();
            return jointpoints.Join(msPoints);
        }

        List<IMSPoint> ConvertPointToMSPoint(List<Point> points, List<IMSPoint> mspoints)
        {
            List<IMSPoint> new_mspoints = new List<IMSPoint>();

            foreach (var point in points)
            {
                foreach (var mspoint in mspoints)
                {
                    if (point == mspoint.GetPoint())
                        new_mspoints.Add(mspoint);
                }
            }

            return new_mspoints;
        }

        Graph ConstructGraph(IEnumerable<IMSPoint> msPoints)
        {
            List<MaskPoint> maskPoints = PrefilterPoints(msPoints);

            List<Point> connectionsBetweenComponents;
            var components = FormInternallyConnectedComponents(maskPoints, out connectionsBetweenComponents);

            Graph graph = new Graph();

            foreach (var component in components)
            {
                for (int i = 0; i < component.Count - 1; i++)
                {
                    graph.AddEdge(component[i].GetPoint(), component[i + 1].GetPoint());
                }
            }

            for (int i = 0; i < connectionsBetweenComponents.Count - 1; i += 2)
            {
                graph.AddEdge(connectionsBetweenComponents[i], connectionsBetweenComponents[i + 1]);
            }

            return graph;
        }

        struct MaskPoint
        {
            public IMSPoint msPoint;
            public IMSPoint ghostedBy;
            public bool IsGhost()
            {
                return ghostedBy != null;
            }
        }
        List<MaskPoint> PrefilterPoints(IEnumerable<IMSPoint> msPoints)
        {
            List<MaskPoint> maskPoints = new List<MaskPoint>(msPoints.Count());
            foreach (var msPoint in msPoints)
            {
                maskPoints.Add(new MaskPoint()
                {
                    msPoint = msPoint,
                    ghostedBy = null
                });
            }

            for (int i = 0; i < maskPoints.Count - 1; i++)
            {
                if (maskPoints[i].IsGhost() || maskPoints[i + 1].IsGhost())
                    continue;

                double distance = (maskPoints[i].msPoint.GetPoint() - maskPoints[i + 1].msPoint.GetPoint()).Length;
                if (i != 0)
                {
                    double prevDistance = (maskPoints[i].msPoint.GetPoint() - maskPoints[i - 1].msPoint.GetPoint()).Length;
                    distance = Math.Min(prevDistance, distance);
                }
                for (int j = i + 1; j < maskPoints.Count; j++)
                {
                    if (j == i + 1 || maskPoints[j].IsGhost())
                        continue;
                    if ((maskPoints[j].msPoint.GetPoint() - maskPoints[i].msPoint.GetPoint()).Length <= distance)
                    {
                        maskPoints[j] = new MaskPoint()
                        {
                            msPoint = maskPoints[j].msPoint,
                            ghostedBy = maskPoints[i].msPoint
                        };
                    }
                }
            }

            for (int i = maskPoints.Count - 1; i > 0; i--)
            {
                if (maskPoints[i - 1].IsGhost() && maskPoints[i].IsGhost())
                {
                    maskPoints.RemoveAt(i - 1);
                }
            }

            if (maskPoints.Last().IsGhost())
                maskPoints.RemoveAt(maskPoints.Count - 1);

            return maskPoints;
        }

        List<List<IMSPoint>> FormInternallyConnectedComponents(List<MaskPoint> mspoints, out List<Point> connectionsBetweenComponents)
        {
            List<List<IMSPoint>> internallyConnectedComponents = new List<List<IMSPoint>>();
            connectionsBetweenComponents = new List<Point>();

            int index = 0;

            while (index < mspoints.Count)
            {
                List<IMSPoint> currentComponent = new List<IMSPoint>();
                while (index < mspoints.Count && !mspoints[index].IsGhost())
                {
                    currentComponent.Add(mspoints[index].msPoint);
                    index++;
                }
                if (index + 1 < mspoints.Count)
                {
                    connectionsBetweenComponents.Add(mspoints[index].ghostedBy.GetPoint());
                    index++;
                    connectionsBetweenComponents.Add(mspoints[index].msPoint.GetPoint());
                }
                if (currentComponent.Count != 0)
                    internallyConnectedComponents.Add(currentComponent);
            }

            return internallyConnectedComponents;
        }

        IMSPoint FindClosestPoint(IEnumerable<IMSPoint> pointList, IMSPoint point)
        {
            int closestPointIndex = 0;
            double minDistance = double.MaxValue;
            for (int i = 0; i < pointList.Count(); i++)
            {
                double currentDistance = (point.GetPoint() - pointList.ElementAt(i).GetPoint()).Length;
                if (currentDistance < minDistance &&
                    point.GetPoint() != pointList.ElementAt(i).GetPoint())
                {
                    closestPointIndex = i;
                    minDistance = currentDistance;
                }
            }
            return pointList.ElementAt(closestPointIndex);
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
