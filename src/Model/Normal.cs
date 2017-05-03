using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.Primitive
{
    public class Normal
    {
        public double dx { get; set; }
        public double dy { get; set; }
        public ISegment segment { get; set; }
        public double t { get; set; }


        public Normal(ISegment segment, double t, double dx, double dy)
        {
            this.segment = segment;
            this.t = t;
            this.dx = dx;
            this.dy = dy;
        }

        public Normal Combine(Normal other, double alpha)
        {
            double dx = this.dx * (1 - alpha) + other.dx * alpha;
            double dy = this.dy * (1 - alpha) + other.dy * alpha;
            double length = Math.Sqrt(dx * dx + dy * dy);
            dx /= length;
            dy /= length;
            if (this.segment == other.segment)
            {
                if (this.t == other.t)
                {
                    return new Normal(this.segment, this.t, dx, dy);
                }
                var t = this.t * (1 - alpha) + other.t * alpha;
                return other.segment.GetNormal(t);
            }
            if (this.segment.GetPillar().First() == other.segment.GetPillar().Last())
            {
                if (this.t == 1 && other.t == 0)
                {
                    return new Normal(this.segment, this.t, dx, dy);
                }
                if (this.t < 1)
                {
                    return this.segment.GetNormal(1);
                }
                return other.segment.GetNormal(0);
            }
            if (this.segment.GetPillar().Last() == other.segment.GetPillar().First())
            {
                if (this.t == 0 && other.t == 1)
                {
                    return new Normal(this.segment, this.t, dx, dy);
                }
                if (other.t < 1)
                {
                    return other.segment.GetNormal(1);
                }
                return this.segment.GetNormal(0);
            }
            throw new Exception("Error: Invalid operation between unrelated segments.");
        }
    }
}
