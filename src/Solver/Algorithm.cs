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
            var lines = new Splitter().Split(solverdata.GetContours(), 0.1);
            IDetailizer detailizer = new Detailizer(lines, mspointfinder, 5);

            IJoinMSPoints jointpoints = new JoinMSPoints(detailizer.Detalize());
            return jointpoints.Join();
        }
    }
}
