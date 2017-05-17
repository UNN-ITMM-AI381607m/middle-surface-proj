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
        double realAccuracy;
        public Splitter()
        {
        }

        public double GetRealAccuracy()
        {
            return realAccuracy;
        }

        public List<ICustomLine> Split(IEnumerable<IContour> contours, double accuracy)
        {
            accuracy = Math.Min(accuracy, 1);
            accuracy = Math.Max(accuracy, 0.0001);
            realAccuracy = accuracy * FindMaxLength(contours, tStep);
            List<ICustomLine> customLines = new List<ICustomLine>();
            int segmentNumber = 0;
            foreach (var contour in contours)
            {
                IEnumerable<ISegment> segments = contour.GetSegments();
                foreach (var segment in segments)
                {
                    List<ICustomPoint> splittedPoints = SplitSegment(segment, realAccuracy, tStep, segmentNumber);

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

        double FindMaxLength(IEnumerable<IContour> contours, double step)
        {
            double maxLength = 0;
            foreach (var contour in contours)
            {
                IEnumerable<ISegment> segments = contour.GetSegments();
                foreach (var segment in segments)
                {
                    double length = 0;
                    double t = 0;
                    while (t < 1)
                    {
                        double nextT = t + step;
                        if (nextT > 1)
                        {
                            nextT = 1;
                        }
                        Point currentPoint = segment.GetCurvePoint(t);
                        Point nextPoint = segment.GetCurvePoint(nextT);
                        length += (currentPoint - nextPoint).Length;

                        t = nextT;
                    }
                    if (maxLength < length)
                    {
                        maxLength = length;
                    }
                }
            }

            return maxLength;
        }

        List<ICustomPoint> SplitSegment(ISegment segment, double accuracy, double step, int segmentNum)
        {
            Point startPoint = segment.GetCurvePoint(0);
            Point lastPoint = segment.GetCurvePoint(1);
            List<ICustomPoint> result = new List<ICustomPoint>() { new CustomPoint(segmentNum, 0, startPoint) };

            double t = 0;
            double prevLength = 0;
            double prevT = t;
            while (t < 1)
            {
                t += step;
                if (t >= 1)
                {
                    break;
                }
                Point currentPoint = segment.GetCurvePoint(t);

                double length = (currentPoint - startPoint).Length;

                if ((prevLength < accuracy && length > accuracy) || length < prevLength)
                {
                    if (t + step >= 1)
                    {
                        double middleT = (1 + prevT) / 2;
                        result.Add(new CustomPoint(segmentNum, middleT, segment.GetCurvePoint(middleT)));
                        break;
                    }
                    result.Add(new CustomPoint(segmentNum, t, currentPoint));
                    startPoint = currentPoint;
                    prevT = t;
                    prevLength = 0;
                }
                else
                    prevLength = length;
            }

            result.Add(new CustomPoint(segmentNum, 1, lastPoint));

            return result;
        }
    }
}
