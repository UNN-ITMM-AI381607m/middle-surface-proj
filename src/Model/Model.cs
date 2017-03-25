using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Model: IModel
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
        
        public List<IFigure> GetData()
        {
            return figures;
        }

        public List<ISegment> GetCanvasData()
        {
            List<ISegment> AllSegments = new List<ISegment>();

            foreach (IFigure figure in figures)
                foreach (IContour contour in figure.GetContours())
                    AllSegments.AddRange(contour.GetSegments());

            return AllSegments;
        }
    }
}
