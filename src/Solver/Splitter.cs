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
        double m_lastPointInaccuracy;
        public Splitter(double lastPointInaccuracy = 0.5)
        {
            m_lastPointInaccuracy = lastPointInaccuracy;
        }

        public List<ICustomLine> Split(IEnumerable<IContour> contours, double accuracy)
        {
            if (accuracy <= 0 || accuracy > 1)
            {
                throw new Exception("Accuracy for splitter must be greater than 0 and less than 1");
            }
            List<ICustomLine> customLines = new List<ICustomLine>();
            foreach (var contour in contours)
            {
                List<ISegment> segments = contour.GetSegments().ToList();
                for (int i = 0; i < segments.Count(); i++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        double nextT = t + accuracy;
                        if (nextT > 1 || (1 - t) < m_lastPointInaccuracy * accuracy)
                        {
                            nextT = 1;
                        }
                        customLines.Add(new CustomLine(new CustomPoint(i, t, segments[i].GetCurvePoint(t)),
                            new CustomPoint(i, nextT, segments[i].GetCurvePoint(nextT))));
                        t = nextT;
                    }
                }
            }
            return customLines;
        }
    }
}
