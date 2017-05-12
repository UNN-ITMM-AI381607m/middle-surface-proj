using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.View;

namespace MidSurfaceNameSpace.Component
{
    public interface IView
    {
        void Paint(IVisibleData data);

        //For debug only
        void EnableIndices(bool enabled);
        void SetIndexFontSize(int size);
    }
}
