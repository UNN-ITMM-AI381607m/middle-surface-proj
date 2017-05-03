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
            public List<IMSPoint> Run(ISolverData solverdata, double splitterAccuracy, double detalizerAccuracy)
            {
                List<IContour> contours = solverdata.GetContours();

                List<ISegment> segments = new List<ISegment>();
                foreach (var contour in contours)
                {
                    segments.AddRange(contour.GetSegments());
                }

                IMSPointFinder mspointfinder = new MSPointFinder(segments);
                var lines = new Splitter().Split(solverdata.GetContours(), splitterAccuracy);
                IDetailizer detailizer = new Detailizer(lines, mspointfinder, segments, detalizerAccuracy);
                mspointfinder.SetLines(lines);
                return mspointfinder.FindMSPoints();
            }
        }

        public IMidSurface Run(ISolverData solverdata)
        {
            IMidSurface midsurface = new MidSurface();

            BaseAlgorithm balg = new BaseAlgorithm();
            List<IMSPoint> MSPoints = balg.Run(solverdata, splitterAccuracy, detalizerAccuracy);

            IJoinMSPoints jointpoints = new JoinMSPoints(MSPoints);//detailizer.Detalize());

            //Graph functions HERE




            return jointpoints.Join();
        }

        public static bool EqualDoubles(double n1, double n2, double precision_)
        {
            return (Math.Abs(n1 - n2) <= precision_);
        }

    }
}
