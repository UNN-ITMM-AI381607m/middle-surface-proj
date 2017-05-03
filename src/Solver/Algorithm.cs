using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class Algorithm : IAlgorithm
    {
        double splitterAccuracy;
        double detalizerAccuracy;

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

                IMSPointFinder mspointfinder = new MSPointFinder(segments);
                simplifiedModel = new Splitter().Split(solverdata.GetContours(), splitterAccuracy);
                IDetailizer detailizer = new Detailizer(simplifiedModel, mspointfinder, segments, detalizerAccuracy);
                mspointfinder.SetLines(simplifiedModel);
                return mspointfinder.FindMSPoints();
            }
        }

        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();

            BaseAlgorithm balg = new BaseAlgorithm();
            List<IMSPoint> MSPoints = balg.Run(solverdata, splitterAccuracy, detalizerAccuracy);

            Graph msGraph = ConstructGraph(MSPoints, balg.GetSimplifiedModel());

            IJoinMSPoints jointpoints = new JoinMSPoints(MSPoints, msGraph);//detailizer.Detalize());
            
            return jointpoints.Join();
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

            for (int i = 0; i < connectionOrder.Count - 1; i++)
            {
                int j = i + 1 == connectionOrder.Count ? 0 : i + 1;
                graph.AddEdge(msPoints.ElementAt(connectionOrder[i]).GetPoint(), msPoints.ElementAt(connectionOrder[j]).GetPoint());
            }

            return graph;
        }

        public static bool EqualDoubles(double n1, double n2, double precision_)
        {
            return (Math.Abs(n1 - n2) <= precision_);
        }

    }
}
