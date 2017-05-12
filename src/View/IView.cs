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
        //addIndex for debug only
        void Paint(IVisibleData data);
        void SetAddIndices(bool enabled);
    }
}
