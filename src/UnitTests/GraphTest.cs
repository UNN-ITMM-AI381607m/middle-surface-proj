using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Solver;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace UnitTests
{
    [TestClass]
    public class GraphTest
    {
        Graph graph;

        public GraphTest()
        {
            graph = new Graph();
        }

        void InitGraph()
        {
            graph.AddEdge(new Point(0, 0), new Point(0, 1));
            graph.AddEdge(new Point(0, 0), new Point(1, 0));
            graph.AddEdge(new Point(1, 0), new Point(0, 1));
        }

        [TestMethod]
        public void InitGraphCheck()
        {
            InitGraph();

            Assert.AreEqual(3, graph.GetEdges().Count());

            List<Graph.Vertex> vertices = graph.GetVertices().ToList();

            Assert.AreEqual(2, vertices[0].neighbours.Count);
            Assert.AreEqual(2, vertices[1].neighbours.Count);
            Assert.AreEqual(2, vertices[2].neighbours.Count);
        }

        [TestMethod]
        public void TestRemoveEdge()
        {
            InitGraph();

            graph.RemoveEdge(new Graph.Edge(new Graph.Vertex(new Point(0, 0)),
                new Graph.Vertex(new Point(0, 1))));

            graph.RemoveEdge(new Graph.Vertex(new Point(0, 0)), new Graph.Vertex(new Point(1, 0)));

            graph.RemoveEdge(new Point(1, 0), new Point(0, 1));

            Assert.AreEqual(0, graph.GetEdges().Count());

            List<Graph.Vertex> vertices = graph.GetVertices().ToList();

            Assert.AreEqual(0, vertices[0].neighbours.Count);
            Assert.AreEqual(0, vertices[1].neighbours.Count);
            Assert.AreEqual(0, vertices[2].neighbours.Count);
        }

        [TestMethod]
        public void TestGetPath()
        {
            graph.AddEdge(new Point(0, 0), new Point(1, 1));
            graph.AddEdge(new Point(1, 1), new Point(2, 2));
            graph.AddEdge(new Point(2, 2), new Point(1, 3));
            graph.AddEdge(new Point(2, 2), new Point(3, 2));
            
            graph.AddEdge(new Point(3, 2), new Point(4, 3));
            graph.AddEdge(new Point(4, 3), new Point(5, 4));
            graph.AddEdge(new Point(3, 2), new Point(4, 1));

            //List<Point> list = new List<Point>(  );
            //list.Add(new Point(0, 0));
            //list.Add(new Point(1, 1));
            //list.Add(new Point(2, 2));
            //list.Add(new Point(3, 2));
            //list.Add(new Point(4, 3));
            //list.Add(new Point(5, 4));
            //List<Point> path = graph.GetPath();
            Assert.AreEqual(6, graph.GetPath().Count);
        }

        [TestMethod]
        public void TestGetAllPaths()
        {
            graph.AddEdge(new Point(1, 3), new Point(2, 4));
            graph.AddEdge(new Point(2, 4), new Point(1, 5));
            graph.AddEdge(new Point(2, 4), new Point(3, 4));
            graph.AddEdge(new Point(3, 4), new Point(4, 4));
            graph.AddEdge(new Point(4, 4), new Point(5, 4));
            graph.AddEdge(new Point(5, 4), new Point(6, 4));

            graph.AddEdge(new Point(6, 4), new Point(7, 3));
            graph.AddEdge(new Point(6, 4), new Point(7, 5));

            Assert.AreEqual(2, graph.GetAllPaths().Count);

            //2
            graph.AddEdge(new Point(3, 1), new Point(4, 2));
            graph.AddEdge(new Point(4, 2), new Point(5, 1));
            graph.AddEdge(new Point(4, 2), new Point(4, 3));
            graph.AddEdge(new Point(4, 3), new Point(4, 4));
            graph.AddEdge(new Point(4, 4), new Point(4, 5));
            graph.AddEdge(new Point(4, 5), new Point(4, 6));

            graph.AddEdge(new Point(4, 6), new Point(3, 7));
            graph.AddEdge(new Point(4, 6), new Point(5, 7));

            Assert.AreEqual(4, graph.GetAllPaths().Count);
        }
        }
}
