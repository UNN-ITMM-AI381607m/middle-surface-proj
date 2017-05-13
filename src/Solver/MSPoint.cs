using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class MSPoint : IMSPoint
    {
        Point mspoint;
        ISegment segment;
        double Radius;
        Point parent;
        Normal normal = null;

        public MSPoint(Point mspoint, ISegment segment)
        {
            this.mspoint = mspoint;
            this.segment = segment;
        }

        public MSPoint(Point mspoint, Point parent, ISegment segment, double R, Normal normal)
        {
            this.mspoint = mspoint;
            this.parent = parent;
            this.segment = segment;
            Radius = R;
            this.normal = normal;
        }

        public Point GetPoint()
        {
            return mspoint;
        }

        public double GetRadius()
        {
            return Radius;
        }

        public Point GetParent()
        {
            return parent;
        }

        public ISegment GetSegment()
        {
            return segment;
        }

        public Normal GetNormal()
        {
            return normal;
        }
    }

    public class MSSegment : ISegment
    {
        ISegment segment;
        List<IMSPoint> pillar;

        public MSSegment(ICurve curve, List<IMSPoint> pillar)
        {
            List<Point> points = new List<Point>();
            this.pillar = pillar;
            foreach (var p in pillar)
                points.Add(p.GetPoint());
            segment = new Segment(curve, points);
        }

        public Point GetCurvePoint(double t)
        {
            return segment.GetCurvePoint(t);
        }

        public List<Point> GetPillar()
        {
            return segment.GetPillar();
        }

        public List<IMSPoint> GetMSPillar()
        {
            return pillar;
        }

        public Normal GetNormal(double t)
        {
            return segment.GetNormal(t);
        }
    }
}
