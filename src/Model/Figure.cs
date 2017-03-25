using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Figure: IFigure
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

        public List<IContour> GetContours()
        {
            return contours;
        }
    }
}
