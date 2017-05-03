using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public interface ICustomLine
    {
        ICustomPoint GetPoint1();
        ICustomPoint GetPoint2();
        Vector GetRightNormal();
        
        void AddMark(int id, Point contactPoint);
        IEnumerable<Mark> GetMarks();
    }
}
