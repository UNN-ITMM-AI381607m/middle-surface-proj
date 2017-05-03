using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    class Detailizer : IDetailizer
    {
        private List<ICustomLine> lines;
        private IMSPointFinder finder;
        private double accuracy;
        private List<ISegment> segments;

        public Detailizer(IMSPointFinder finder, List<ICustomLine> lines, List<ISegment> segments, double accuracy)
        {
            this.lines = lines;
            this.finder = finder;
            this.segments = segments;
            this.accuracy = accuracy;
        }

        //public List<IMSPoint> Detalize()
        //{
        //    List<IMSPoint> mspoints = new List<IMSPoint>();
        //    IMSPoint mspoint1 = null, mspoint2 = null;
        //    bool previousPointAdded = false;

        //    finder.SetLines(lines);

        //    for (int i = 0; i < lines.Count(); ++i)
        //    {
        //        int j = i == lines.Count() - 1 ? 0 : i + 1;
        //        mspoint1 = previousPointAdded ? mspoint2 : GetMSPoint(i);
        //        mspoint2 = GetMSPoint(j);

        //        // different contours
        //        if (lines[i].GetPoint2().GetPoint() != lines[j].GetPoint1().GetPoint() ||
        //        // suitable for accuracy
        //            !DetailRequired(mspoint1, mspoint2))
        //        {
        //            mspoints.Add(mspoint1);
        //            previousPointAdded = true;
        //        }
        //        else
        //        {
        //            var vector1 = lines[i].GetPoint1().GetPoint() - lines[i].GetPoint2().GetPoint();
        //            var vector2 = lines[j].GetPoint2().GetPoint() - lines[j].GetPoint1().GetPoint();

        //            bool line1IsPoint4Bisector = lines[i].GetPoint1().GetPoint() == lines[i].GetPoint2().GetPoint();
        //            bool line2IsPoint4Bisector = lines[j].GetPoint1().GetPoint() == lines[j].GetPoint2().GetPoint();

        //            // bisector case
        //            if (Vector.AngleBetween(vector1, vector2) < 0 &&
        //                lines[i].GetPoint1().GetN() != lines[j].GetPoint2().GetN())
        //            {
        //                if (lines.Count() > i + 1)
        //                {
        //                    lines.Insert(i + 1, new CustomLine(lines[i].GetPoint2(), lines[j].GetPoint1()));
        //                }
        //                else
        //                {
        //                    lines.Add(new CustomLine(lines[i].GetPoint2(), lines[j].GetPoint1()));
        //                }
        //            }
        //            else
        //            {
        //                var newLines = SplitOnLines(lines[i], lines[j]);
        //                if (newLines == null)
        //                {
        //                    mspoints.Add(mspoint1);
        //                    continue;
        //                }

        //                int index = line1IsPoint4Bisector ? i + 1 : i;
        //                var line = lines[j];

        //                if (!line1IsPoint4Bisector) lines.Remove(lines[i]);
        //                if (!line2IsPoint4Bisector) lines.Remove(line);
        //                if (lines.Count() > index)
        //                {
        //                    lines.InsertRange(index, newLines);
        //                }
        //                else
        //                {
        //                    lines.AddRange(newLines);
        //                }
        //            }
        //            i -= (j == 0 && !line2IsPoint4Bisector) ? 2 : 1;
        //            previousPointAdded = false;
        //        }
        //    }
        //    return mspoints;
        //}

        //public bool DetailRequired(IMSPoint point1, IMSPoint point2)
        //{
        //    var midpointsDistance = (point2.GetPoint() - point1.GetPoint()).Length;
        //    // If mspoints are suitable for accuracy 
        //    if (midpointsDistance <= accuracy) return false;

        //    return true;
        //}

        //private List<ICustomLine> SplitOnLines(ICustomLine line1, ICustomLine line2)
        //{
        //    var point1 = new CustomPoint(line1.GetPoint1().GetN(), line1.GetPoint1().GetT(),
        //         new Point((line1.GetPoint1().GetPoint().X + line1.GetPoint2().GetPoint().X) / 2,
        //       (line1.GetPoint1().GetPoint().Y + line1.GetPoint2().GetPoint().Y) / 2));

        //    var point2 = new CustomPoint(line2.GetPoint1().GetN(), line2.GetPoint1().GetT(),
        //         new Point((line2.GetPoint1().GetPoint().X + line2.GetPoint2().GetPoint().X) / 2,
        //       (line2.GetPoint1().GetPoint().Y + line2.GetPoint2().GetPoint().Y) / 2));

        //    if ((point1.GetPoint() - point2.GetPoint()).Length < 0.0001)
        //    {
        //        return null;
        //    }
        //    var newLine1 = new CustomLine(line1.GetPoint1(), point1);
        //    var newLine2 = new CustomLine(point1, line1.GetPoint2());
        //    var newLine3 = new CustomLine(line2.GetPoint1(), point2);
        //    var newLine4 = new CustomLine(point2, line2.GetPoint2());
        //    var list = new List<ICustomLine>();

        //    bool line1IsPoint4Bisector = line1.GetPoint1().GetPoint() == line1.GetPoint2().GetPoint();
        //    bool line2IsPoint4Bisector = line2.GetPoint1().GetPoint() == line2.GetPoint2().GetPoint();
        //    if (!line1IsPoint4Bisector)
        //    {
        //        list.Add(newLine1);
        //        list.Add(newLine2);
        //    }
        //    if (!line2IsPoint4Bisector)
        //    {
        //        list.Add(newLine3);
        //        list.Add(newLine4);
        //    }
        //    return list;
        //}

        private IMSPoint GetMSPoint(ICustomLine line, ref Normal normal)
        {
              Point middlePoint = new Point((line.GetPoint1().GetPoint().X + line.GetPoint2().GetPoint().X) / 2,
                  (line.GetPoint1().GetPoint().Y + line.GetPoint2().GetPoint().Y) / 2);
            normal = segments[line.GetPoint1().GetN()].GetNormal((line.GetPoint1().GetT() + line.GetPoint2().GetT()) / 2);

            return finder.FindMSPoint(middlePoint, normal);
        }

        // TO DO: implement new method and make it main method
        public List<IMSPoint> Detalize()
        {
            finder.SetLines(lines);
            List<IMSPoint> mspoints = new List<IMSPoint>();

            IMSPoint mspoint1 = null, mspoint2 = null;
            bool firstAdded = false;
            Normal n1 = null, n2 = null;

            for (int i = 0; i < lines.Count; i++)
            {
                int j = i == lines.Count() - 1 ? 0 : i + 1;
                if (firstAdded)
                {
                    mspoint1 = mspoint2;
                    n1 = n2;
                }
                else mspoint1 = GetMSPoint(lines[i], ref n1);
                mspoint2 = GetMSPoint(lines[j], ref n2);

                mspoints.Add(mspoint1);
                firstAdded = true;

                var line1middlePoint = new Point((lines[i].GetPoint1().GetPoint().X + lines[j].GetPoint2().GetPoint().X) / 2,
                       (lines[i].GetPoint1().GetPoint().Y + lines[j].GetPoint2().GetPoint().Y) / 2);

                // different contours
                if (lines[i].GetPoint2().GetPoint() != lines[j].GetPoint1().GetPoint() ||
                // suitable for accuracy
                 (mspoint2.GetPoint() - mspoint1.GetPoint()).Length <= accuracy ||
                 // If distance to midpoint is more than  midpoints distance
                 (line1middlePoint - mspoint1.GetPoint()).Length <= (mspoint2.GetPoint() - mspoint1.GetPoint()).Length)
                {
                    continue;
                }
                else
                {
                    DetalizeChunk(ref mspoints, mspoint1.GetPoint(), mspoint2.GetPoint(), n1, n2);
                }
            }
            return mspoints;
        }

        private void DetalizeChunk(ref List<IMSPoint> points, Point point1, Point point2, Normal n1, Normal n2)
        {
            if ((point2 - point1).Length <= accuracy) return;

            var seg1N = segments.IndexOf(n1.Segment());
            var seg2N = segments.IndexOf(n2.Segment());
            var n = n1.Combine(n2);

            var mspoint = finder.FindMSPoint(n1.Segment().GetCurvePoint((n1.T() + n2.T()) / 2), n);
            if ((mspoint.GetPoint() - point1).Length <= accuracy) DetalizeChunk(ref points, point1, mspoint.GetPoint(), n1, n);
            if ((point2 - mspoint.GetPoint()).Length <= accuracy) DetalizeChunk(ref points, mspoint.GetPoint(), point2, n, n2);
            points.Add(mspoint);
        }
    }
}