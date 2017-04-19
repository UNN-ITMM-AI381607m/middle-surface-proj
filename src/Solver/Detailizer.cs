using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    class Detailizer : IDetailizer
    {
        private IMSPointFinder finder;
        private double accuracy;

        public Detailizer(IMSPointFinder finder, double accuracy)
        {
            this.finder = finder;
            this.accuracy = accuracy;
        }

        public List<IMSPoint> Detalize()
        {
            var lines = new List<ICustomLine>();
            var mspoints = finder.FindMSPoints();

            foreach (var point in mspoints)
            {
                lines.Add(point.GetLine());
            }
            for (int i = 0; i < mspoints.Count(); i++)
            {
                int j = i == mspoints.Count() - 1 ? 0 : i + 1;

                if (DetailRequired(mspoints[i], mspoints[j]))
                {
                    var line1 = mspoints[i].GetLine();
                    var line2 = mspoints[j].GetLine();
                    var vector1 = line1.GetPoint1().GetPoint() - line1.GetPoint2().GetPoint();
                    var vector2 = line2.GetPoint2().GetPoint() - line2.GetPoint1().GetPoint();

                    if (Vector.AngleBetween(vector1, vector2) < 0 &&
                        line1.GetPoint1().GetN() != line2.GetPoint2().GetN())
                    {
                        var bisector = line1.GetRightNormal() + line2.GetRightNormal();
                        bisector.Normalize();
                        var newLine = new CustomLine(line1.GetPoint2(), line2.GetPoint1());
                        mspoints.Insert(j, finder.FindMSPointForLine(newLine, bisector));
                    }
                    else
                    {
                        var newLines = SplitOnLines(line1, line2);
                        if (newLines == null) continue;
                   
                        bool point1FromBisector = line1.GetPoint1().GetPoint() == line1.GetPoint2().GetPoint();
                        bool point2FromBisector = line2.GetPoint1().GetPoint() == line2.GetPoint2().GetPoint();

                        int index = point1FromBisector ? lines.IndexOf(line2) : lines.IndexOf(line1);
                        lines.Remove(line1);
                        lines.Remove(line2);
                        if (lines.Count() > index)
                        {
                            lines.InsertRange(index, newLines);
                        }
                        else
                        {
                            lines.AddRange(newLines);
                        }
                        finder.SetLines(lines);

                        int insertIndex = i;
                        var mspoint1 = mspoints[i];
                        var mspoint2 = mspoints[j];
                        if (!point2FromBisector)
                        {
                            insertIndex = j;
                            mspoints.Remove(mspoint2);
                        }
                        if (!point1FromBisector)
                        {
                            insertIndex = i;
                            mspoints.Remove(mspoint1);
                        }
                        for (int k = 0; k < newLines.Count; k++)
                        {
                            mspoints.Insert(insertIndex + k, finder.FindMSPointForLine(newLines[k]));
                        }
                    }
                    i--;
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

            if ((point1.GetPoint() - point2.GetPoint()).Length < 0.01)
            {
                return null;
            }
            var newLine1 = new CustomLine(line1.GetPoint1(), point1);
            var newLine2 = new CustomLine(point1, line1.GetPoint2());
            var newLine3 = new CustomLine(line2.GetPoint1(), point2);
            var newLine4 = new CustomLine(point2, line2.GetPoint2());
            var list = new List<ICustomLine>();

            bool point1FromBisector = line1.GetPoint1().GetPoint() == line1.GetPoint2().GetPoint();
            bool point2FromBisector = line2.GetPoint1().GetPoint() == line2.GetPoint2().GetPoint();
            if (!point1FromBisector)
            {
                list.Add(newLine1);
                list.Add(newLine2);
            }
            if (!point2FromBisector)
            {
                list.Add(newLine3);
                list.Add(newLine4);
            }
            return list;
        }
    }
}