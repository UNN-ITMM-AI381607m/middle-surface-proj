﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Quality
{
    public class Quality: IQuality
    {
        ITestQuality test;

        public Quality(ITestQuality test)
        {
            this.test = test;
        }

        public void Check(IModel model, IMidSurface midsurface)
        {
            test.CheckQuality(model, midsurface);
        }
    }
}
