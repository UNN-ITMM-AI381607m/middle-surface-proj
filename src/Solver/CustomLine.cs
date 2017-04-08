using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class CustomLine: ICustomLine
    {
        ICustomPoint firstpoint;
        ICustomPoint secondpoint;
        Vector vector;

        public CustomLine(ICustomPoint firstpoint, ICustomPoint secondpoint, Vector vector)
        {
            this.firstpoint = firstpoint;
            this.secondpoint = secondpoint;
            this.vector = vector;
        }

        public ICustomPoint GetPoint1()
        {
            return firstpoint;
        }

        public ICustomPoint GetPoint2()
        {
            return secondpoint;
        }

        public Vector GetNormal()
        {
            return vector;
        }
    }
}
