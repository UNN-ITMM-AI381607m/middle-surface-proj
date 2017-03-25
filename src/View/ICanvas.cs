using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace View
{
    public interface ICanvas
    {
        void Draw(ISegment segment);
    }
}
