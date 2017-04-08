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
        ICustomPoint point1;
        ICustomPoint point2;
        Vector rightNormalVector;

        public CustomLine(ICustomPoint firstPoint, ICustomPoint secondPoint)
        {
            point1 = firstPoint;
            point2 = secondPoint;
            CalculateNormalVector();
        }

        public ICustomPoint GetPoint1()
        {
            return point1;
        }

        public ICustomPoint GetPoint2()
        {
            return point2;
        }

        public Vector GetRightNormal()
        {
            return rightNormalVector;
        }

        void CalculateNormalVector()
        {
            double dx = point2.GetPoint().X - point1.GetPoint().X;
            double dy = point2.GetPoint().Y - point1.GetPoint().Y;
            rightNormalVector = new Vector(dy, -dx);
        }
    }
}
