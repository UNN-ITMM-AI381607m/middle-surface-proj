using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Solver
{
    public class Algorithm : IAlgorithm
    {
        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();
            ISplitter splitter = new Splitter();
            IMSPointFinder mspointfinder = new MSPointFinder(/*solverdata.GetSegments()*/null);
            IJoinMSPoints jointpoints = new JoinMSPoints();

            return jointpoints.Join(mspointfinder, mspointfinder.FindMSPoints(splitter.Split(solverdata.GetContours(), 0.1)), 5.0);
        }
    }
}
