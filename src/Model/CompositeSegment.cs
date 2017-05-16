using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Primitive
{
    public class CompositeSegment:ISegment
    {
        List<ISegment> segments;

        public CompositeSegment()
        {
            segments = new List<ISegment>();
        }

        public void Add(ISegment segment)
        {
            segments.Add(segment);
        }

        public Point GetCurvePoint(double t)
        {
            if (t <= 0.5)
                return segments[0].GetCurvePoint(t * 2);
            else
                return segments[1].GetCurvePoint((t - 0.5) * 2);
        }

        public Normal GetNormal(double t)
        {
            if (t <= 0.5)
                return segments[0].GetNormal(t * 2);
            else if (t == 0.5)
                return segments[1].GetNormal(0).Combine(segments[0].GetNormal(1));
            else
                return segments[1].GetNormal((t - 0.5) * 2);
        }

        public List<Point> GetPillar()
        {
            List<Point> pillars = segments[0].GetPillar();
            pillars.AddRange(segments[1].GetPillar());
            return pillars;
        }

        public void Remove(int num)
        {
            segments.RemoveAt(num);
        }

        
    }
}
