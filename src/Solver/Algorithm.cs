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

        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();

            List<IContour> contours = solverdata.GetContours();

            List<ISegment> segments = new List<ISegment>();
            foreach (var contour in contours)
            {
                segments.AddRange(contour.GetSegments());
            }

            IMSPointFinder mspointfinder = new MSPointFinder(segments);
            var lines = new Splitter().Split(solverdata.GetContours(), splitterAccuracy);
            IDetailizer detailizer = new Detailizer(lines, mspointfinder, detalizerAccuracy);

            IJoinMSPoints jointpoints = new JoinMSPoints(detailizer.Detalize());
            return jointpoints.Join();
        }
    }
}
