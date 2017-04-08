using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class CustomPoint: ICustomPoint
    {
        int N;
        double t;
        double alpha;
        IPointF normalPoint;

        public CustomPoint(int N, double t, double alpha=0)
        {
            this.N = N;
            this.t = t;
            this.alpha = alpha;
        }

        public CustomPoint(int N, double t)
        {
            this.N = N;
            this.t = t;
        }

        public CustomPoint(int N, double t, double alpha, IPointF normalPoint)
        {
            this.N = N;
            this.t = t;
            this.alpha = alpha;
            this.normalPoint = normalPoint;
        }

        public int GetN()
        {
            return N;
        }

        public double GetT()
        {
            return t;
        }

        public double GetAlpha()
        {
            return alpha;
        }

        public IPointF GetNormal()
        {
            return normalPoint;
        }
    }
}
