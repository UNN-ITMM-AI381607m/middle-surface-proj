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
            List<ICustomLine> customLines = new List<ICustomLine>();
            foreach (var contour in contours)
            {
                List<ISegment> segments = contour.GetSegments().ToList();
                for (int i = 0; i < segments.Count(); i++)
                {
                    double t = 0;
                    while (t <= 1 - accuracy)
                    {
                        customLines.Add(new CustomLine(new CustomPoint(i, t, segments[i].GetCurvePoint(t)),
                            new CustomPoint(i, t + accuracy, segments[i].GetCurvePoint(t + accuracy))));

                        t += accuracy;
                    }
                    if (1 - t + accuracy <= m_lastPointInaccuracy * accuracy)
                    {
                        customLines.RemoveAt(customLines.Count() - 1);
                    }
                    if (customLines.Last().GetPoint2().GetT() != 1)
                        customLines.Add(new CustomLine(customLines.Last().GetPoint2(),
                        new CustomPoint(i, 1, segments[i].GetCurvePoint(1))));
                }
            }
            return customLines;
        }
    }
}
