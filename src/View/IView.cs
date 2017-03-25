using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MidSurface;

namespace View
{
    public interface IView
    {
        void Paint(IModel model);
        void Paint(IMidSurface midsurface);
    }
}
