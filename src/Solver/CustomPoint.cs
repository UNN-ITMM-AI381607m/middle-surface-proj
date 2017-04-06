using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class CustomPoint: ICustomPoint
    {
        int N;
        double t;
        double alpha;

        public CustomPoint(int N, double t, double alpha=0)
        {
            this.N = N;
            this.t = t;
            this.alpha = alpha;
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
    }
}
