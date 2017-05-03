using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Primitive;
using MidSurfaceNameSpace.IO;
using MidSurfaceNameSpace.Component;
using System.Windows;

namespace UnitTests
{
    [TestClass]
    public class MSModelTest
    {
        [TestMethod]
        public void TestCanvasData()
        {
            List<System.Windows.Point> pillar = new List<System.Windows.Point>();
            System.Windows.Point p1 = new System.Windows.Point();
            p1.X = 1;
            p1.Y = 1;
            System.Windows.Point p2 = new System.Windows.Point();
            p2.X = 2;
            p2.Y = 2;
            pillar.Add(p1);
            pillar.Add(p2);
            ISegment segment = new Segment(new BezierCurve(), pillar);
            IContour contour = new MidSurfaceNameSpace.Primitive.Contour();
            contour.Add(segment);
            IFigure figure = new Figure();
            figure.Add(contour);
            IModel model = new Model();
            model.Add(figure);
            MSModel msmodel = new MSModel(model);
            List<bool> removed = new List<bool>();
            removed.Add(true);
            msmodel.Removed = removed;
            int counter = msmodel.GetCanvasData().Count();
            Assert.AreEqual(counter, 0);
        }
    }
}
