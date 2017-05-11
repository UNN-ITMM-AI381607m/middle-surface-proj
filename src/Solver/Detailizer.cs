﻿using System;
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
    //    private List<IMSPoint> normals = new List<IMSPoint>();

        public Detailizer(IMSPointFinder finder, List<ICustomLine> lines, List<ISegment> segments, double accuracy)
        {
            this.lines = lines;
            this.finder = finder;
            this.segments = segments;
            this.accuracy = accuracy;
        }

        private IMSPoint GetMSPoint(ICustomLine line1, ICustomLine line2, ref Normal normal)
        {
            if (line1.GetPoint1().GetN() != line2.GetPoint1().GetN())
            {
                if (segments[line1.GetPoint1().GetN()].GetPillar().First() == segments[line2.GetPoint2().GetN()].GetPillar().Last())
                {
                    normal = segments[line1.GetPoint1().GetN()].GetNormal(0).Combine(segments[line2.GetPoint2().GetN()].GetNormal(1));
                    return finder.FindMSPoint(line1.GetPoint1().GetPoint(), normal);
                }
            }

            normal = segments[line1.GetPoint1().GetN()].GetNormal(line1.GetPoint1().GetT());
            return finder.FindMSPoint(line1.GetPoint1().GetPoint(), normal);
        }

        public List<IMSPoint> Detalize()
        {
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
              //  normals.Add(new MSPoint(new Point(lines[i].GetPoint1().GetPoint().X + n1.Dx() * 3, lines[i].GetPoint1().GetPoint().Y + n1.Dy() * 3), null));
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
            if ((point2 - point1).Length <= accuracy) return false;
            //TO DO: move to MSPointFinder 
            if ((n1.Segment().GetCurvePoint(n1.T()) - point1).Length <= accuracy) return false;
            if ((n2.Segment().GetCurvePoint(n2.T()) - point2).Length <= accuracy) return false;
            return true;
        }

        private void DetalizeChunk(ref List<IMSPoint> points, Point point1, Point point2, Normal n1, Normal n2)
        {
            var seg1N = segments.IndexOf(n1.Segment());
            var seg2N = segments.IndexOf(n2.Segment());
            Point middlePoint;
            Normal n;
            if (seg1N != seg2N)
            {
                var newT = n1.T() + Normal.T_OFFSET;
                if (newT >= 1) return;
                middlePoint = n1.Segment().GetCurvePoint(newT);
                n = n1.Segment().GetNormal(newT);
            }
            else
            {
                middlePoint = n1.Segment().GetCurvePoint((n1.T() + n2.T()) / 2);
                n = n1.Combine(n2);
            }
            var mspoint = finder.FindMSPoint(middlePoint, n);

            if (DetailRequired(point1, mspoint.GetPoint(), n1, n)) DetalizeChunk(ref points, point1, mspoint.GetPoint(), n1, n);
            points.Add(mspoint);
          //  normals.Add(new MSPoint(new Point(middlePoint.X + n.Dx() * 3, middlePoint.Y + n.Dy() * 3), null));
            if (DetailRequired(mspoint.GetPoint(), point2, n, n2)) DetalizeChunk(ref points, mspoint.GetPoint(), point2, n, n2);
        }
    }
}