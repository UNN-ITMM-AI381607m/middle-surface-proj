﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public interface IMSPoint
    {
        Point GetPoint();
        ISegment GetSegment();
        double GetRadius();
        Normal GetNormal();
        Point GetParent();
    }
}
