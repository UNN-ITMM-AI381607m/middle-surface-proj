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

        public Detailizer(List<ICustomLine> lines,
            IMSPointFinder finder, double accuracy)
        {
            this.lines = lines;
            this.finder = finder;
            this.accuracy = accuracy;
        }

        public List<IMSPoint> Detalize()
        {
            List<IMSPoint> mspoints = new List<IMSPoint>();
            IMSPoint mspoint1 = null, mspoint2 = null;
            bool previousPointAdded = false;

            for (int i = 0; i < lines.Count(); ++i)
            {
                int j = i == lines.Count() - 1 ? 0 : i + 1;
  
                finder.SetLines(lines);

                mspoint1 = previousPointAdded ? mspoint2 : GetMSPoint(i);
                mspoint2 = GetMSPoint(j);

                if (!DetailRequired(mspoint1, mspoint2))
                {
                    mspoints.Add(mspoint1);
                    previousPointAdded = true;
                }
                else
                {
                    var vector1 = lines[i].GetPoint1().GetPoint() - lines[i].GetPoint2().GetPoint();
                    var vector2 = lines[j].GetPoint2().GetPoint() - lines[j].GetPoint1().GetPoint();

                    bool line1IsPoint4Bisector = lines[i].GetPoint1().GetPoint() == lines[i].GetPoint2().GetPoint();
                    bool line2IsPoint4Bisector = lines[j].GetPoint1().GetPoint() == lines[j].GetPoint2().GetPoint();

                    // bisector case
                    if (Vector.AngleBetween(vector1, vector2) < 0 &&
                        lines[i].GetPoint1().GetN() != lines[j].GetPoint2().GetN())
                    {
                        if (lines.Count() > i + 1)
                        {
                            lines.Insert(i + 1, new CustomLine(lines[i].GetPoint2(), lines[j].GetPoint1()));
                        }
                        else
                        {
                            lines.Add(new CustomLine(lines[i].GetPoint2(), lines[j].GetPoint1()));
                        }
                    }
                    else
                    {
                        var newLines = SplitOnLines(lines[i], lines[j]);
                        if (newLines == null)
                        {
                            mspoints.Add(mspoint1);
                            continue;
                        }
                   
                        int index = line1IsPoint4Bisector ? i + 1 : i;
                        var line = lines[j];

                        if (!line1IsPoint4Bisector) lines.Remove(lines[i]);
                        if (!line2IsPoint4Bisector) lines.Remove(line);
                        if (lines.Count() > index)
                        {
                            lines.InsertRange(index, newLines);
                        }
                        else
                        {
                            lines.AddRange(newLines);
                        }
                    }
                    i -= (j == 0 && !line2IsPoint4Bisector) ? 2 : 1;
                    previousPointAdded = false;
                }
            }
            return mspoints;
        }

        public bool DetailRequired(IMSPoint point1, IMSPoint point2)
        {
            var line1 = point1.GetLine();
            var line2 = point2.GetLine();

            // If mspoints belong to different contours
            if (line1.GetPoint2().GetPoint() != line2.GetPoint1().GetPoint()) return false;

            var midpointsDistance = (point2.GetPoint() - point1.GetPoint()).Length;
            // If mspoints are suitable for accuracy 
            if (midpointsDistance <= accuracy) return false;

            var line1middlePoint = new Point((line1.GetPoint1().GetPoint().X + line1.GetPoint2().GetPoint().X) / 2,
                  (line1.GetPoint1().GetPoint().Y + line1.GetPoint2().GetPoint().Y) / 2);

            // If distance to midpoint is more than  midpoints distance
            if ((line1middlePoint - point1.GetPoint()).Length - midpointsDistance > 0) return false;
            return true;
        }

        private List<ICustomLine> SplitOnLines(ICustomLine line1, ICustomLine line2)
        {
            var point1 = new CustomPoint(line1.GetPoint1().GetN(), line1.GetPoint1().GetT(),
                 new Point((line1.GetPoint1().GetPoint().X + line1.GetPoint2().GetPoint().X) / 2,
               (line1.GetPoint1().GetPoint().Y + line1.GetPoint2().GetPoint().Y) / 2));

            var point2 = new CustomPoint(line2.GetPoint1().GetN(), line2.GetPoint1().GetT(),
                 new Point((line2.GetPoint1().GetPoint().X + line2.GetPoint2().GetPoint().X) / 2,
               (line2.GetPoint1().GetPoint().Y + line2.GetPoint2().GetPoint().Y) / 2));

            if ((point1.GetPoint() - point2.GetPoint()).Length < 0.0001)
            {
                return null;
            }
            var newLine1 = new CustomLine(line1.GetPoint1(), point1);
            var newLine2 = new CustomLine(point1, line1.GetPoint2());
            var newLine3 = new CustomLine(line2.GetPoint1(), point2);
            var newLine4 = new CustomLine(point2, line2.GetPoint2());
            var list = new List<ICustomLine>();

            bool line1IsPoint4Bisector = line1.GetPoint1().GetPoint() == line1.GetPoint2().GetPoint();
            bool line2IsPoint4Bisector = line2.GetPoint1().GetPoint() == line2.GetPoint2().GetPoint();
            if (!line1IsPoint4Bisector)
            {
                list.Add(newLine1);
                list.Add(newLine2);
            }
            if (!line2IsPoint4Bisector)
            {
                list.Add(newLine3);
                list.Add(newLine4);
            }
            return list;
        }

        private IMSPoint GetMSPoint(int lineIndex)
        {
            var line = lines[lineIndex];
            int prevIndex = lineIndex == 0 ? lines.Count() - 1 : lineIndex - 1;
            int nextIndex = lineIndex == lines.Count() - 1 ? 0 : lineIndex + 1;

            if (line.GetPoint1().GetPoint() == line.GetPoint2().GetPoint())
            {
                var bisector = lines[prevIndex].GetRightNormal() + lines[nextIndex].GetRightNormal();
                bisector.Normalize();
                return finder.FindMSPointForLine(line, bisector);
            }
            return finder.FindMSPointForLine(line);
        }
    }
}