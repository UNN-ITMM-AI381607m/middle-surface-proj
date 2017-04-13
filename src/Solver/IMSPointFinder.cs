using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.Solver
{
    public interface IMSPointFinder
    {

        void SetLines(List<ICustomLine> lines);
        List<IMSPoint> FindMSPoints();
        IMSPoint FindMSPointForLine(ICustomLine line);
        //IMSPoint GetMSPoint(Vector vector, Point point, ICustomLine line, List<ICustomLine> lines);
        List<ISegment> GetSegments();
    }
}
