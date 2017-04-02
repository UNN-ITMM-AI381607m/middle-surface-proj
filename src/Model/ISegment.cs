﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Primitive
{
    public interface ISegment
    {
        IPointF GetCurvePoint(double t);
        IEnumerable<IPointF> GetPillar();
    }
}