using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Component
{
    public interface IView
    {
        void Paint(IModel model);
        void Paint(IMidSurface midsurface);
    }
}
