using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Model
{
    public interface ICurve
    {
        PointF GetCurvePoint(List<PointF> pillar, double t); 
    }
}
