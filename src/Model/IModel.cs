using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Component
{
    public interface IModel
    {
        void Add(IFigure figure);
        IEnumerable<IFigure> GetData();
        IEnumerable<ISegment> GetCanvasData();
        Size GetSize();
    }
}
