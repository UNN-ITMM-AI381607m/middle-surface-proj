using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class Splitter : ISplitter
    {
        const double tStep = 0.01;
        const double lastPartAccuracy = 0.7;
        public Splitter()
        {
        }

        public List<ICustomLine> Split(IEnumerable<IContour> contours, double accuracy)
        {
            List<ICustomLine> customLines = new List<ICustomLine>();
            int segmentNumber = 0;
            foreach (var contour in contours)
            {
                IEnumerable<ISegment> segments = contour.GetSegments();
                foreach (var segment in segments)
                {
                    List<ICustomPoint> splittedPoints = SplitSegment(segment, accuracy, tStep, segmentNumber);

                    for (int i = 0; i < splittedPoints.Count - 1; i++)
                    {
                        ICustomLine newLine = new CustomLine(splittedPoints[i], splittedPoints[i + 1]);
                        customLines.Add(newLine);
                    }
                    segmentNumber++;
                }
            }
            return customLines;
        }

        List<ICustomPoint> SplitSegment(ISegment segment, double accuracy, double step, int segmentNum)
        {
            Point startPoint = segment.GetCurvePoint(0);
            Point lastPoint = segment.GetCurvePoint(1);
            List<ICustomPoint> result = new List<ICustomPoint>() { new CustomPoint(segmentNum, 0, startPoint) };

            double t = 0;
            double startT = t;
            while (t < 1)
            {
                double nextT = t + step;
                if (nextT > 1)
                {
                    nextT = 1;
                }
                Point currentPoint = segment.GetCurvePoint(t);
                Point nextPoint = segment.GetCurvePoint(nextT);

                double length = (currentPoint - startPoint).Length;
                double nextLength = (nextPoint - startPoint).Length;

                if ((length < accuracy && nextLength > accuracy) || nextLength < length)
                {
                    result.Add(new CustomPoint(segmentNum, nextT, nextPoint));
                    startPoint = nextPoint;
                    startT = nextT;
                }
                t = nextT;
            }

            if (result.Count > 1 && (lastPoint - result.Last().GetPoint()).Length < lastPartAccuracy * accuracy)
            {
                result.RemoveAt(result.Count - 1);
                double middleT = (1 + result.Last().GetT()) / 2;
                result.Add(new CustomPoint(segmentNum, middleT, segment.GetCurvePoint(middleT)));
            }

            result.Add(new CustomPoint(segmentNum, 1, lastPoint));

            return result;
        }
    }
}
