using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public partial class MSPointFinder : IMSPointFinder
    {
        List<ISegment> segments;

        public MSPointFinder(List<ISegment> segments)
        {
            this.segments = segments;
        }

        public List<IMSPoint> FindMSPoints(List<ICustomLine> custompoints)
        {
            List<IMSPoint> mspoints = new List<IMSPoint>();

            /*List<CustomPoint> linear = new List<CustomPoint>();
            double Rmax = getRmax();
            for (int i = 0; i < custompoints.Count(); i++)
            {
                List<double> normal = new List<double>(2);
                int j = (i + 1 == custompoints.Count()) ? 0 : i + 1;
                IPointF parrent_1 = segments[custompoints[i].GetN()].GetCurvePoint(custompoints[i].GetT());
                IPointF parrent_2 = segments[custompoints[j].GetN()].GetCurvePoint(custompoints[j].GetT());
                double X_1 = parrent_1.GetX();
                double X_2 = parrent_2.GetX();
                double Y_1 = parrent_1.GetY();
                double Y_2 = parrent_2.GetY();
                double k = 0;
                if (X_1 == X_2)
                {
                    normal.Add((Y_2 > Y_1) ? 1 : -1);
                    normal.Add(0);
                }
                else if (Y_1 == Y_2)
                {
                    normal.Add(0);
                    normal.Add((X_2 > X_1) ? -1 : 1);
                }
                else
                {
                    k = (X_2 - X_1) / (Y_2 - Y_1);
                    normal.Add(k / (Math.Sqrt(k * k + 1)));
                    normal.Add(-1 / (Math.Sqrt(k * k + 1)));
                    
                }
                linear.Add(new CustomPoint(custompoints[i].GetN(), custompoints[i].GetT(), k));

                double R = Rmax;
                double X = (X_2 + X_1) / 2;
                double Y = (Y_2 + Y_1) / 2;


                IPointF center = new PointF(X + normal[0] * R, Y + normal[1] * R);
                List<PointF> pCross = cross(center, R, new PointF(X, Y));
                while (pCross.Count != 1)
                {
                    if (pCross.Count >= 2)
                    {
                        R = R / 2;
                    }
                    else
                    {
                        R = R * 3 / 2;
                    }
                    
                    center = new PointF(X + normal[0] * R, Y + normal[1] * R);
                    pCross = cross(center, R, new PointF(X, Y));
                }
                ICustomPoint first_parent = new CustomPoint(linear[i].GetN(), linear[i].GetT(), linear[i].GetAlpha());
                ICustomPoint second_parent = new CustomPoint(custompoints[j].GetN(), custompoints[j].GetT(), custompoints[j].GetAlpha());

                mspoints.Add(new MSPoint(center, first_parent, second_parent));
            }
            */
            return mspoints;
        }

        List<Point> cross(Point center, double rad, Point rivol)
        {
            List<Point> result = new List<Point>();
            /*const double e = 0.1;
            for (int i = 0; i < segments.Count; i++)
            {
                for (double t = 0; t <= 1; t += 0.05)
                {
                    IPointF point = segments[i].GetCurvePoint(t);
                    //if (Math.Abs(point.GetX() - rivol.GetX()) > e || Math.Abs(point.GetY() - rivol.GetY()) > e) // наверное надо сравнивать по компонентам
                    //{
                    //    if (Math.Abs(rad * rad - Math.Pow(point.GetX() - center.GetX(), 2) - Math.Pow(point.GetY() - center.GetY(), 2)) <= e) // попали в окрестность контура окружности можем уточнить половинным делением, потом...
                    //    {
                    //        result.Add(new PointF(point.GetX(), point.GetY()));
                    //    }
                    //    else if (Math.Pow(point.GetX() - center.GetX(), 2) + Math.Pow(point.GetY() - center.GetY(), 2) < Math.Pow(rad, 2))
                    //        result.Add(new PointF(point.GetX(), point.GetY()));
                    //}
                    if (point.GetX() != rivol.GetX() || point.GetY() != rivol.GetY()) // наверное надо сравнивать по компонентам
                    {
                        //if (Math.Abs(rad * rad - Math.Pow(point.GetX() - center.GetX(), 2) - Math.Pow(point.GetY() - center.GetY(), 2)) <= e) // попали в окрестность контура окружности можем уточнить половинным делением, потом...
                        //{
                        //    result.Add(new PointF(point.GetX(), point.GetY()));
                        //}
                        //else 
                        if (Math.Pow(point.GetX() - center.GetX(), 2) + Math.Pow(point.GetY() - center.GetY(), 2) <= Math.Pow(rad, 2))
                            result.Add(new PointF(point.GetX(), point.GetY()));
                    }

                    if (result.Count >= 2)// здесь бы тоже проверить на две точки
                        break; // нам достаточно только две точки
                }

                if (result.Count >= 2)
                    break; // нам достаточно только две точки
            }*/

            return result;
        }

        double getRmax()
        {
            double R, Xmax = 0, Xmin = double.MaxValue, Ymax = 0, Ymin = double.MaxValue;
            for (int i = 0; i < segments.Count; i++)
            {
                double X = segments[i].GetCurvePoint(0.0).X;
                double Y = segments[i].GetCurvePoint(0.0).Y;
                if (X < Xmin)
                    Xmin = X;
                else if (X > Xmax)
                    Xmax = X;
                if (Y < Ymin)
                    Ymin = Y;
                else if (Y > Ymax)
                    Ymax = Y;
            }
            R = Math.Max(Xmax - Xmin, Ymax - Ymin);
            return R;
        }
    }
}
