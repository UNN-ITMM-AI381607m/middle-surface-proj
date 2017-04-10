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
            ISplitter splitter = new Splitter();
            List < IContour > contours = solverdata.GetContours();
            List<ISegment> segments = new List<ISegment>();
            foreach (var contour in contours)
            {
                segments.AddRange(contour.GetSegments());
            }
            IMSPointFinder mspointfinder = new MSPointFinder(segments);
            IJoinMSPoints jointpoints = new JoinMSPoints();

            return jointpoints.Join(mspointfinder, mspointfinder.FindMSPoints(splitter.Split(solverdata.GetContours(), 0.1)), 15.0);
        }
    }
}
