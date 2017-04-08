using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class Splitter: ISplitter
    {
        double m_lastPointInaccuracy;
        public Splitter(double lastPointInaccuracy = 0.1) {
            m_lastPointInaccuracy = lastPointInaccuracy;
        }

        public List<ICustomPoint> Split(IEnumerable<ISegment> segments, double accuracy)
        {
            List<ICustomPoint> customPoints = new List<ICustomPoint>();
            for (int i = 0; i < segments.Count(); i++)
            {
                double t = 0;
                while (t <= 1)
                {
                    customPoints.Add(new CustomPoint(i, t));
                    t += accuracy;
                }
                if (1 - t + accuracy <= m_lastPointInaccuracy * accuracy)
                {
                    customPoints.RemoveAt(customPoints.Count() - 1);
                }
                customPoints.Add(new CustomPoint(i, 1));
            }
            return customPoints;
        }
    }
}
