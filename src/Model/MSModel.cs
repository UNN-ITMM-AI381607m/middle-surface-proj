using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Component
{
    public class MSModel : IModel
    {
        IModel model;
        List<bool> removed;

        public MSModel(IModel model)
        {
            this.model = model;
            this.removed = new List<bool>();
            List<ISegment> segm = model.GetCanvasData().ToList();
            foreach (var seg in segm)
            {
                removed.Add(false);
            }
        }

        public void Add(IFigure figure)
        {
            model.Add(figure);
        }

        public IEnumerable<IFigure> GetData()
        {
            IModel new_model = new Model();
            IEnumerable<IFigure> figures = model.GetData();

            int i = 0;

            foreach (IFigure figure in figures)
            {
                IFigure new_figure = new Figure();
                foreach (IContour contour in figure.GetContours())
                {
                    IContour new_contour = new Contour();
                    foreach(ISegment segment in contour.GetSegments())
                    {
                        if (!removed[i])
                            new_contour.Add(segment);
                        i++;
                    }
                    new_figure.Add(new_contour);
                }
                new_model.Add(new_figure);
            }

            return new_model.GetData();
        }

        public IEnumerable<ISegment> GetCanvasData()
        {
            List<ISegment> AllSegments = new List<ISegment>();

            foreach (IFigure figure in model.GetData())
                foreach (IContour contour in figure.GetContours())
                    AllSegments.AddRange(contour.GetSegments());

            List<ISegment> new_segments = new List<ISegment>();

            for(int i=0;i<AllSegments.Count();i++)
            {
                if (!removed[i])
                    new_segments.Add(AllSegments[i]);
            }

            return new_segments;
        }

        public List<bool> Removed
        {
            set{ removed = value; }
        }

        public Size GetSize()
        {
            return model.GetSize();
        }
    }
}
