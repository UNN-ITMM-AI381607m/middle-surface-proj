using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PointF: IPointF
    {
        double X;
        double Y;

        public PointF() { }

        public PointF(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public double GetX()
        {
            return X;
        }

        public double GetY()
        {
            return Y;
        }
    }
}
