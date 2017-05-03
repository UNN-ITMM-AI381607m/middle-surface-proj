using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class Graph
    {
        public class Vertex
        {
            public Point point;
            public List<Vertex> neighbours;
            public Vertex(Point p)
            {
                point = p;
                neighbours = new List<Vertex>();
            }

            public Vertex(Point p, List<Vertex> a/*, double w*/)
            {
                point = p;
                neighbours = a;
                //weight = w;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Vertex p = obj as Vertex;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public bool Equals(Vertex p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public override int GetHashCode()
            {
                return point.GetHashCode();
            }

            public static bool operator ==(Vertex a, Vertex b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.point == b.point;
            }

            public static bool operator ==(Vertex a, Point b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.point == b;
            }

            public static bool operator !=(Vertex a, Vertex b)
            {
                return !(a == b);
            }

            public static bool operator !=(Vertex a, Point b)
            {
                return !(a == b);
            }
        }

        public class Edge
        {
            public Vertex vertex1;
            public Vertex vertex2;
            public Edge(Vertex n1, Vertex n2)
            {
                vertex1 = n1;
                vertex2 = n2;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Edge p = obj as Edge;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public bool Equals(Edge p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public override int GetHashCode()
            {
                return vertex1.GetHashCode() ^ vertex2.GetHashCode();
            }

            public static bool operator ==(Edge a, Edge b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return (a.vertex1 == b.vertex1 && a.vertex2 == b.vertex2) || (a.vertex1 == b.vertex2 && a.vertex2 == b.vertex1);
            }

            public static bool operator !=(Edge a, Edge b)
            {
                return !(a == b);
            }
        }

        List<Vertex> vertices;
        List<Edge> edges;
        List<int> foundCycle;

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public IEnumerable<Vertex> GetVertices()
        {
            return vertices;
        }

        public IEnumerable<Edge> GetEdges()
        {
            return edges;
        }

        public void AddEdge(Point p1, Point p2)
        {
            AddEdge(new Vertex(p1), new Vertex(p2));
        }

        public void AddEdge(Edge edgeToAdd)
        {
            AddEdge(edgeToAdd.vertex1, edgeToAdd.vertex2);
        }

        public void AddEdge(Vertex vertex1, Vertex vertex2)
        {
            if (vertex1 == vertex2 || ContainsEdge(vertex1, vertex2))
                return;

            Vertex vertexToConnect1 = null;
            Vertex vertexToConnect2 = null;

            if (!vertices.Any(x => x == vertex1 || x == vertex2))
            {
                vertices.Add(vertex1);
                vertices.Add(vertex2);
                vertexToConnect1 = vertex1;
                vertexToConnect2 = vertex2;
            }
            else
            {
                Vertex foundVertex1 = vertices.Find(x => x == vertex1);
                Vertex foundVertex2 = vertices.Find(x => x == vertex2);
                if (foundVertex1 == null)
                {
                    vertices.Add(vertex1);
                }
                else if (foundVertex2 == null)
                {
                    vertices.Add(vertex2);
                }
                vertexToConnect1 = foundVertex1 ?? vertex1;
                vertexToConnect2 = foundVertex2 ?? vertex2;
            }

            edges.Add(new Edge(vertexToConnect1, vertexToConnect2));
            vertexToConnect1.neighbours.Add(vertexToConnect2);
            vertexToConnect2.neighbours.Add(vertexToConnect1);
        }

        public void RemoveEdge(Edge edgeToRemove)
        {
            Edge edge = edges.Find(x => x == edgeToRemove);

            if (edge == null)
                return;

            edge.vertex1.neighbours.Remove(edge.vertex2);
            edge.vertex2.neighbours.Remove(edge.vertex1);

            edges.Remove(edge);
        }

        public void RemoveEdge(Vertex vertex1, Vertex vertex2)
        {
            RemoveEdge(new Edge(vertex1, vertex2));
        }

        public void RemoveEdge(Point p1, Point p2)
        {
            RemoveEdge(new Edge(new Vertex(p1), new Vertex(p2)));
        }

        bool ContainsEdge(Vertex vertex1, Vertex vertex2)
        {
            Edge constructedEdge = new Edge(vertex1, vertex2);
            foreach (var edge in edges)
            {
                if (edge == constructedEdge)
                    return true;
            }
            return false;
        }

        bool ContainsEdge(Point p1, Point p2)
        {
            return ContainsEdge(new Vertex(p1), new Vertex(p2));
        }

        public void AddVertex(Point p)
        {
            bool searchP = false;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].point == p)
                {
                    searchP = true;
                    break;
                }
            }
            if (!searchP)
                vertices.Add(new Vertex(p, new List<Vertex>()));
        }

        //public void maxPath(List<Point> path, double maxWeight)
        //{
        //    foreach(var node in nodes)
        //    {
        //        if (node.adj.Count == 1 && path.FindIndex(new Predicate<Point>(s => s == node.adj[0])) != -1)
        //            break;
        //        foreach(Point a in node.adj)
        //        {
        //            if(path.FindIndex(new Predicate<Point>(s=> s == a)) == -1)
        //            {
        //                path.Add(a);
        //                maxWeight += getWeight(node.point, a);
        //                maxPath(path, maxWeight);
        //            }

        //        }
        //    }
        //}

        //public void setWeight()
        //{
        //    double weight
        //    foreach (Point a in nodes[0].adj) // 
        //}

        public List<Point> GetPath()
        {
            List<Point> path = new List<Point>();
            //double maxWeight = 0;
            //int indexStart = 0;
            //for(int i = 0; i < nodes.Count; i++)
            //{
            //    if(nodes[i].weight > maxWeight)
            //    {
            //        maxWeight = nodes[i].weight;
            //        indexStart = i;
            //    }
            //}
            //path.Add(nodes[indexStart].point);
            int i = 0;
            for (; i < vertices.Count; i++)
            {
                if (vertices[i].neighbours.Count == 1)
                {
                    path.Add(vertices[i].point);
                    path.Add(vertices[i].neighbours[0].point);
                    break;
                }
            }

            return path;
        }

        public double GetWeight(Point p1, Point p2)
        {
            return Math.Pow(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2), 0.5);
        }

        public void RemoveCycles()
        {
            while (true)
            {
                SearchCycle();
                if (foundCycle.Count == 0)
                    return;

                double xNew = 0;
                double yNew = 0;
                for (int i = 0; i < foundCycle.Count; i++)
                {
                    int j = i + 1 == foundCycle.Count ? 0 : i + 1;
                    xNew += vertices[foundCycle[i]].point.X;
                    yNew += vertices[foundCycle[i]].point.Y;
                    RemoveEdge(vertices[foundCycle[i]], vertices[foundCycle[j]]);
                }
                xNew /= foundCycle.Count;
                yNew /= foundCycle.Count;
                Vertex newVertex = new Vertex(new Point(xNew, yNew));

                foreach (var index in foundCycle)
                {
                    if (vertices[index].neighbours.Count != 0)
                        AddEdge(vertices[index], newVertex);
                }
            }
        }

        private void SearchCycle()
        {
            foundCycle = new List<int>();
            int[] color = new int[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int k = 0; k < vertices.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>
                {
                    i
                };
                if (DFScycle(i, i, edges, color, -1, cycle))
                    return;
            }
        }

        private bool DFScycle(int u, int endV, List<Edge> edges, int[] color, int unavailableEdge, List<int> cycle)
        {
            if (u != endV)
                color[u] = 2;
            else if (cycle.Count > 2)
            {
                cycle.Reverse();
                foundCycle.AddRange(cycle.Take(cycle.Count - 1));
                return true;
            }
            for (int w = 0; w < edges.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[vertices.IndexOf(edges[w].vertex2)] == 1 && vertices.IndexOf(edges[w].vertex1) == u)
                {
                    List<int> cycleCopy = new List<int>(cycle)
                    {
                        vertices.IndexOf(edges[w].vertex2)
                    };
                    if (DFScycle(vertices.IndexOf(edges[w].vertex2), endV, edges, color, w, cycleCopy))
                        return true;
                    color[vertices.IndexOf(edges[w].vertex2)] = 1;
                }
                else if (color[vertices.IndexOf(edges[w].vertex1)] == 1 && vertices.IndexOf(edges[w].vertex2) == u)
                {
                    List<int> cycleCopy = new List<int>(cycle)
                    {
                        vertices.IndexOf(edges[w].vertex1)
                    };
                    if (DFScycle(vertices.IndexOf(edges[w].vertex1), endV, edges, color, w, cycleCopy))
                        return true;
                    color[vertices.IndexOf(edges[w].vertex1)] = 1;
                }
            }
            return false;
        }
    }
}
