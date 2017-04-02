using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;
using View;

namespace MidSurface.Component
{
    public class View: IView
    {
        ICanvas canvas;

        public View(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Paint(IVisibleData data)
        {
            foreach (ISegment segment in data.GetSegments())
                canvas.Draw(segment);
        }

    }
}
