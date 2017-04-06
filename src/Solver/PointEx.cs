using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace Solver
{
    public class PointEx
    {
        List<ICustomPoint> parents;
        IPointF mspoint;

        public PointEx(IPointF mspoint, ICustomPoint first_parent, ICustomPoint second_parent)
        {
            this.mspoint = mspoint;
            parents = new List<ICustomPoint>();
            parents.Add(first_parent);
            parents.Add(second_parent);
        }

        public IPointF GetMSPoint()
        {
            return mspoint;
        }

        public ICustomPoint GetFirstParent()
        {
            return parents[0];
        }

        public ICustomPoint GetSecondParent()
        {
            return parents[1];
        }
    }
}
