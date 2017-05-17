using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.Solver;

namespace MidSurfaceNameSpace
{
    public class Segments_iterator
    {
        List<ISegment> segments;
        bool[] is_used;
        Contour tmp_count;
        Figure tmp_fig;
        Model tmp_model;
        int k, kolvo;
        public Segments_iterator(IModel pmodel)
        {
            SolverData SD = new SolverData(pmodel);
            segments = new List<ISegment>();
            foreach (var contour in SD.GetContours())
                segments.AddRange(contour.GetSegments());
            is_used = new bool[segments.Count];
            for (int i = 0; i < is_used.Length; i++) is_used[i] = false;
        }
        public IModel Next
        {
            get
            {
                k = 0;
                while (is_used[k] == true) k++;
                is_used[k] = true;
                for (int i = 0; i < k; i++) is_used[i] = false;

                tmp_count = new Contour();
                for (int i = 0; i < is_used.Length; i++)
                    if (is_used[i]) tmp_count.Add(segments[i]);
                tmp_fig = new Figure();
                tmp_fig.Add(tmp_count);
                tmp_model = new Model();
                tmp_model.Add(tmp_fig);
                return tmp_model;
            }
        }
        public bool IsOver
        {
            get
            {
                kolvo = 0;
                foreach (bool b in is_used)
                    if (b) kolvo++;
                if (kolvo != is_used.Length) return false;
                else return true;
            }
        }

    }
}
