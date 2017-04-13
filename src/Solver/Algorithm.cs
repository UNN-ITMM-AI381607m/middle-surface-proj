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

            ISplitter splitter = new Splitter(10);
            IMSPointFinder mspointfinder = new MSPointFinder(segments);

            mspointfinder.SetLines(splitter.Split(solverdata.GetContours(), 0.1));

            IJoinMSPoints jointpoints = new JoinMSPoints(mspointfinder, segments, 20);
            return jointpoints.Join();
        }
    }
}
