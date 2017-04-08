using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Solver
{
    public class SolverData: ISolverData
    {
        List<IContour> contours;

        public SolverData(IModel model)
        {
            contours = new List<IContour>();
            List<IFigure> figures = new List<IFigure>();
            foreach (var figure in figures)
            {
                contours.AddRange(figure.GetContours());
            }
        }

        public List<IContour> GetContours()
        {
            return contours;
        }
    }
}
