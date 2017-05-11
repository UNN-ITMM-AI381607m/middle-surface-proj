using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace MidSurfaceNameSpace.Solver
{
    public class Averager : IAverager
    {
        public class LinearSegment
        {
            public double K { get; }
            public double B { get; }
            public Point StartPoint { get; }

            public double Len { get; }

            public int Direction { get; }

            public LinearSegment(Point first, Point second)
            {
                Direction = 1;
                if ((first.X > second.X && first.Y > second.Y) || (first.X > second.X && first.Y < second.Y))
                    Direction = -1;

                if (second.X - first.X == 0)
                    second.X += 0.000000000001; //это плохо)

                K = (second.Y - first.Y) / (second.X - first.X);
                B = first.Y - K * first.X;
                StartPoint = first;
                Len = Math.Sqrt((second.X - first.X) * (second.X - first.X) + (second.Y - first.Y) * (second.Y - first.Y));
            }

            public Point GetPointByDistance(double t)
            {
                var x = Direction * Math.Sqrt((t * t) / ((1 + K * K))) + StartPoint.X;
                var y = K * x + B;
                return new Point(x, y);
            }
        }

        public int Accuracy { get; set; }

        private List<List<LinearSegment>> _linearPaths;

        public List<List<LinearSegment>> LinearPaths { get { return _linearPaths; } }

        public void Init(List<List<Point>> paths)
        {
            _linearPaths = new List<List<LinearSegment>>();
            foreach (var path in paths)
            {
                var linearPath = new List<LinearSegment>();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    linearPath.Add(new LinearSegment(path[i], path[i + 1]));
                }
                _linearPaths.Add(linearPath);
            }
        }

        public Point GetPointByDistance(List<LinearSegment> path, double t)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (t > path[i].Len)
                {
                    t -= path[i].Len;
                    continue;
                }
                return path[i].GetPointByDistance(t);
            }
            throw new Exception("This is not meant to be");
        }

        public double GetPathLength(List<LinearSegment> path)
        {
            var len = 0.0;
            foreach (var segment in path)
                len += segment.Len;
            return len;
        }

        public List<Point> GetAveragePath(List<List<Point>> paths)
        {
            Init(paths);
            var griddedPaths = new List<List<Point>>();
            var result = new List<Point>();
            foreach (var p in _linearPaths)
            {
                griddedPaths.Add(GridPath(p));
            }
            for (int j = 0; j < griddedPaths[0].Count; j++)
            {
                var sumPoint = new Point(0, 0);
                for (int i = 0; i < griddedPaths.Count; i++)
                {
                    sumPoint.X += griddedPaths[i][j].X;
                    sumPoint.Y += griddedPaths[i][j].Y;
                }
                sumPoint.X /= griddedPaths.Count;
                sumPoint.Y /= griddedPaths.Count;
                result.Add(sumPoint);
            }
            return result;
        }

        public List<Point> GridPath(List<LinearSegment> path)
        {
            if (Accuracy == 0) throw new Exception("Accuracy == 0!");
            var len = GetPathLength(path);
            var step = len / Accuracy;
            var pathPoints = new List<Point>();
            for (int i = 0; i < Accuracy; i++)
            {
                pathPoints.Add(GetPointByDistance(path, step * i));
            }
            return pathPoints;
        }

    }
}
