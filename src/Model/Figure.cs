using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurface.Primitive
{
    public class Figure: IFigure
    {
        List<IContour> contours;

        public Figure()
        {
            contours = new List<IContour>();
        }

        public void Add(IContour contour)
        {
            contours.Add(contour);
        }

        public IEnumerable<IContour> GetContours()
        {
            return contours;
        }

        public IEnumerator<IContour> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
