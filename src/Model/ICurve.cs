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
        IPointF GetCurvePoint(IEnumerable<IPointF> pillar, double t); 
    }
}
