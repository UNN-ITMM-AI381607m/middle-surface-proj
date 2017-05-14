using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class Detailizer : IDetailizer
    {
        private List<ICustomLine> lines;
        private IMSPointFinder finder;
        private double accuracy;
        private List<ISegment> segments;
        static int stackCounter;
        const int stackSize = 2000;

        public Detailizer(IMSPointFinder finder, List<ICustomLine> lines, List<ISegment> segments, double accuracy)
        {
            this.lines = lines;
            this.finder = finder;
            this.segments = segments;
            this.accuracy = accuracy > 1 ? 100 : accuracy * 100;
            stackCounter = 0;
        }

        private IMSPoint GetMSPoint(ICustomLine nextLine, ICustomLine prevLine, ref Normal normal)
        {
            if (nextLine.GetPoint1().GetN() != prevLine.GetPoint1().GetN())
            {
                if (segments[nextLine.GetPoint1().GetN()].GetPillar().First() == segments[prevLine.GetPoint2().GetN()].GetPillar().Last())
                {
                    normal = segments[nextLine.GetPoint1().GetN()].GetNormal(0).Combine(segments[prevLine.GetPoint2().GetN()].GetNormal(1));
                    return finder.FindMSPoint(nextLine.GetPoint1().GetPoint(), normal);
                }
            }

            normal = segments[nextLine.GetPoint1().GetN()].GetNormal(nextLine.GetPoint1().GetT());
            return finder.FindMSPoint(nextLine.GetPoint1().GetPoint(), normal);
        }

        public List<IMSPoint> Detalize()
        {
            if (finder == null) return new List<IMSPoint>();

            List<IMSPoint> mspoints = new List<IMSPoint>();

            IMSPoint mspoint1 = null, mspoint2 = null;
            bool firstAdded = false;
            Normal n1 = null, n2 = null;

            for (int i = 0; i < lines.Count; i++)
            {
                int j = i == lines.Count() - 1 ? 0 : i + 1;
                int k = i == 0 ? lines.Count - 1 : i - 1;

                if (firstAdded)
                {
                    mspoint1 = mspoint2;
                    n1 = n2;
                }
                else mspoint1 = GetMSPoint(lines[i], lines[k], ref n1);
                mspoint2 = GetMSPoint(lines[j], lines[i], ref n2);

                mspoints.Add(mspoint1);
                firstAdded = true;

                if (lines[i].GetPoint2().GetPoint() == lines[j].GetPoint1().GetPoint() &&
                    DetailRequired(mspoint1.GetPoint(), mspoint2.GetPoint(), n1, n2))
                {
                    DetalizeChunk(ref mspoints, mspoint1.GetPoint(), mspoint2.GetPoint(), n1, n2);
                }
            }

            return mspoints;
        }

        private bool DetailRequired(Point point1, Point point2, Normal n1, Normal n2)
        {
            if (accuracy <= -1) return false;

            if ((point2 - point1).Length <= accuracy) return false;
            //TO DO: move to MSPointFinder 
            if ((n1.Segment().GetCurvePoint(n1.T()) - point1).Length <= accuracy) return false;
            if ((n2.Segment().GetCurvePoint(n2.T()) - point2).Length <= accuracy) return false;
            return true;
        }

        private void DetalizeChunk(ref List<IMSPoint> points, Point point1, Point point2, Normal n1, Normal n2, Point prevPoint = new Point())
        {
            //Workaround to prevent StackOverflowed exception
            if (stackCounter > stackSize)
                return;

            stackCounter++;

            if (segments.IndexOf(n1.Segment()) != segments.IndexOf(n2.Segment()))
            {
                n2 = new Normal(n1.Segment(), 1, n2.Dx(), n2.Dy()); 
            }

            var n = n1.Combine(n2);
            var middlePoint = n1.Segment().GetCurvePoint(n.T());

            var mspoint = finder.FindMSPoint(middlePoint, n);

            if (DetailRequired(point1, mspoint.GetPoint(), n1, n)) DetalizeChunk(ref points, point1, mspoint.GetPoint(), n1, n);
            points.Add(mspoint);
            if (DetailRequired(mspoint.GetPoint(), point2, n, n2)) DetalizeChunk(ref points, mspoint.GetPoint(), point2, n, n2);

            stackCounter--;
        }
    }
}