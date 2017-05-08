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

            BaseAlgorithm baseAlgorithm = new BaseAlgorithm();
            List<IMSPoint> msPoints = baseAlgorithm.Run(solverdata, splitterAccuracy, detalizerAccuracy);

            Graph msGraph = ConstructGraph(msPoints, baseAlgorithm.GetSimplifiedModel());
            //msGraph.RemoveCycles(maxCycleSize);

            List<bool> no_point = new List<bool>();
            foreach (var mspoint in msPoints)
            {
                no_point.Add(false);
            }
            List<ICustomLine> lines = baseAlgorithm.GetSimplifiedModel().ToList();
            foreach (var line in lines)
            {
                foreach(var mark in line.GetMarks())
                {
                    Vector vector1 = new Vector(mark.ContactPoint.X - msPoints[mark.MSPointIndex].GetPoint().X, mark.ContactPoint.Y - msPoints[mark.MSPointIndex].GetPoint().Y);
                    Vector vector2 = new Vector(msPoints[mark.MSPointIndex].GetChild().X - msPoints[mark.MSPointIndex].GetPoint().X, msPoints[mark.MSPointIndex].GetChild().Y - msPoints[mark.MSPointIndex].GetPoint().Y);
                    Double angleBetween;
                    angleBetween = Vector.AngleBetween(vector1, vector2);
                    if (Math.Abs(angleBetween) < 95)
                    { }
                    else { no_point[mark.MSPointIndex] = true; }
                }
            }
            List<IMSPoint> new_new = new List<IMSPoint>();
            for(int i=0;i<msPoints.Count();i++)
            {
                if (no_point[i])
                    new_new.Add(msPoints[i]);
            }
           

            

            List<Point> points = msGraph.GetPath();
            /*List<Point> points = new List<Point>();
            foreach (var vert in msGraph.GetVertices())
            {
                points.Add(vert.point);
            }*/
            List<IMSPoint> new_mspoints = ConvertPointToMSPoint(points, new_new);



            List<bool> removed = new List<bool>();
            List<IContour> contours = solverdata.GetContours();

            List<ISegment> segments = new List<ISegment>();
            foreach (var contour in contours)
            {
                segments.AddRange(contour.GetSegments());
            }
            for (int i=0;i<segments.Count();i++)
            {
                removed.Add(true);
            }

            int counter = 0;
            foreach (var segment in segments)
            {
                foreach (var mspoint in new_mspoints)
                {
                    if (mspoint.GetSegment().GetPillar().SequenceEqual(segment.GetPillar()))
                        removed[counter] = false;
                }
                counter++;
            }
            ISolverData new_solverdata = new CustomSolverData(solverdata, removed);

            List<IMSPoint> imsPoints = baseAlgorithm.Run(new_solverdata, splitterAccuracy, detalizerAccuracy);
            Graph imsGraph = ConstructGraph(imsPoints, baseAlgorithm.GetSimplifiedModel());
            imsGraph.RemoveCycles(maxCycleSize);

            IJoinMSPoints jointpoints = new JoinMSPoints(imsGraph);

            return jointpoints.Join();
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
            foreach (var line in simplifiedModel)
            {
                var marksOnLine = line.GetMarks();
                foreach (var mark in marksOnLine)
                {
                    if (connectionOrder.Count == 0 || mark.MSPointIndex != connectionOrder.Last())
                        connectionOrder.Add(mark.MSPointIndex);
                }
            }

            for (int i = 0; i < connectionOrder.Count; i++)
            {
                int j = i + 1 == connectionOrder.Count ? 0 : i + 1;
                graph.AddEdge(msPoints.ElementAt(connectionOrder[i]).GetPoint(), msPoints.ElementAt(connectionOrder[j]).GetPoint());
            }

            //for (int i = 0; i < msPoints.Count(); i++)
            //{
            //    int j = i + 1 == msPoints.Count() ? 0 : i + 1;
            //    graph.AddEdge(msPoints.ElementAt(i).GetPoint(), msPoints.ElementAt(j).GetPoint());
            //}

            return graph;
        }

        public static bool EqualDoubles(double n1, double n2, double precision_)
        {
            return (Math.Abs(n1 - n2) <= precision_);
        }

    }
}
