using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Component
{
    public class Model: IModel
    {
        List<IFigure> figures;

        public Model()
        {
            figures = new List<IFigure>();
        }

        public void Add(IFigure figure)
        {
            figures.Add(figure);
        }
        
        public IEnumerable<IFigure> GetData()
        {
            return figures;
        }

        public IEnumerable<ISegment> GetCanvasData()
        {
            List<ISegment> AllSegments = new List<ISegment>();

            foreach (IFigure figure in figures)
                foreach (IContour contour in figure.GetContours())
                    AllSegments.AddRange(contour.GetSegments());

            return AllSegments;
        }
    }
}
