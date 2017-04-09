using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Component
{
    public struct Size
    {
        public double Xmin;
        public double Xmax;
        public double Ymin;
        public double Ymax;
    }

    public class Model: IModel
    {
        List<IFigure> figures;
        Size size;

        public Model()
        {
            figures = new List<IFigure>();
        }

        public void Add(IFigure figure)
        {
            figures.Add(figure);
            getRmax();
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

        public Size GetSize()
        {
            return size;
        }

        void getRmax()
        {
            double Xmax = double.MinValue, Xmin = double.MaxValue, Ymax = double.MinValue, Ymin = double.MaxValue;
            List<ISegment> segments = GetCanvasData().ToList();
            for (int i = 0; i < segments.Count; i++)
            {
                double X = segments[i].GetCurvePoint(0.0).X;
                double Y = segments[i].GetCurvePoint(0.0).Y;
                if (X < Xmin)
                    Xmin = X;
                else if (X > Xmax)
                    Xmax = X;
                if (Y < Ymin)
                    Ymin = Y;
                else if (Y > Ymax)
                    Ymax = Y;
            }

            size.Xmin = Xmin;
            size.Xmax = Xmax;
            size.Ymin = Ymin;
            size.Ymax = Ymax;
        }
    }
}
