using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public class CustomSolverData: ISolverData
    {
        ISolverData solverdata;
        List<bool> removed;
        
        public CustomSolverData(ISolverData solverdata, List<bool> removed)
        {
            this.solverdata = solverdata;
            this.removed = removed;
        }

        public List<IContour> GetContours()
        {
            IFigure figures = new Figure();
            List<IContour> contours = solverdata.GetContours();
            int i = 0;

            foreach (var contour in contours)
            {
                IContour new_contour = new Contour();
                foreach (ISegment segment in contour.GetSegments())
                {
                    if (!removed[i])
                        new_contour.Add(segment);
                    i++;
                }
                figures.Add(new_contour);
            }

            return figures.GetContours().ToList();
        }

        public List<bool> Removed
        {
            set { removed = value; }
        }
    }
}
