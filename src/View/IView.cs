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


        void EnableIndices(bool enabled);
        void SetIndexFontSize(int size);
#if DEBUG
        void ChangeZoom(double zoom);
        void ChangeCenter(System.Windows.Point p );
#endif
    }
}
