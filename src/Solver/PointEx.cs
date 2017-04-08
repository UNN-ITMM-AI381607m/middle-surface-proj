using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class PointEx:IPointEx
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

        public List<ICustomPoint> GetParents()
        {
            return parents;
        }

        public double GetDistance(IPointEx p)
        {
            return Math.Sqrt(Math.Pow(mspoint.GetX() - p.GetMSPoint().GetX(), 2) + Math.Pow(mspoint.GetY() - p.GetMSPoint().GetY(), 2));
        }
    }
}
