using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class Splitter : ISplitter
    {
        public Splitter()
        {
        }

        public List<ICustomLine> Split(IEnumerable<IContour> contours, double accuracy)
        {
            List<ICustomLine> customLines = new List<ICustomLine>();
            int segmentNumber = 0;
            foreach (var contour in contours)
            {
                List<ISegment> segments = contour.GetSegments().ToList();
                for (int i = 0; i < segments.Count(); i++, segmentNumber++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        double nextT = t + accuracy;
                        if (nextT > 1 || 1 - nextT < accuracy)
                        {
                            nextT = 1;
                        }
                        customLines.Add(new CustomLine(new CustomPoint(segmentNumber, t, segments[i].GetCurvePoint(t)),
                            new CustomPoint(segmentNumber, nextT, segments[i].GetCurvePoint(nextT))));
                        t = nextT;
                    }
                }
            }
            return customLines;
        }
    }
}
