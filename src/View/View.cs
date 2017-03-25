using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MidSurface;

namespace View
{
    public class View: IView
    {
        ICanvas canvas;

        public View(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Paint(IModel model)
        {
            foreach (ISegment segment in model.GetCanvasData())
                canvas.Draw(segment);
        }

        public void Paint(IMidSurface midsurface)
        {
            foreach (ISegment segment in midsurface.GetData())
                canvas.Draw(segment);
        }
    }
}
