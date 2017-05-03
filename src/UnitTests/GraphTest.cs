using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.Solver;
using System.Linq;
using System.Collections.Generic;

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
            graph.AddEdge(new System.Windows.Point(0, 0), new System.Windows.Point(0, 1));
            graph.AddEdge(new System.Windows.Point(0, 0), new System.Windows.Point(1, 0));
            graph.AddEdge(new System.Windows.Point(1, 0), new System.Windows.Point(0, 1));
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

            graph.RemoveEdge(new Graph.Edge(new Graph.Vertex(new System.Windows.Point(0, 0)),
                new Graph.Vertex(new System.Windows.Point(0, 1))));

            graph.RemoveEdge(new Graph.Vertex(new System.Windows.Point(0, 0)), new Graph.Vertex(new System.Windows.Point(1, 0)));

            graph.RemoveEdge(new System.Windows.Point(1, 0), new System.Windows.Point(0, 1));

            Assert.AreEqual(0, graph.GetEdges().Count());

            List<Graph.Vertex> vertices = graph.GetVertices().ToList();

            Assert.AreEqual(0, vertices[0].neighbours.Count);
            Assert.AreEqual(0, vertices[1].neighbours.Count);
            Assert.AreEqual(0, vertices[2].neighbours.Count);
        }

        [TestMethod]
        public void TestGetPath()
        {
            graph.AddEdge(new System.Windows.Point(0, 0), new System.Windows.Point(1, 1));
            graph.AddEdge(new System.Windows.Point(1, 1), new System.Windows.Point(2, 2));
            graph.AddEdge(new System.Windows.Point(2, 2), new System.Windows.Point(1, 3));
            graph.AddEdge(new System.Windows.Point(2, 2), new System.Windows.Point(3, 2));
            
            graph.AddEdge(new System.Windows.Point(3, 2), new System.Windows.Point(4, 3));
            graph.AddEdge(new System.Windows.Point(4, 3), new System.Windows.Point(5, 4));
            graph.AddEdge(new System.Windows.Point(3, 2), new System.Windows.Point(4, 1));

            //List<System.Windows.Point> list = new List<System.Windows.Point>(  );
            //list.Add(new System.Windows.Point(0, 0));
            //list.Add(new System.Windows.Point(1, 1));
            //list.Add(new System.Windows.Point(2, 2));
            //list.Add(new System.Windows.Point(3, 2));
            //list.Add(new System.Windows.Point(4, 3));
            //list.Add(new System.Windows.Point(5, 4));
            //List<System.Windows.Point> path = graph.GetPath();
            Assert.AreEqual(6, graph.GetPath().Count);
        }
    }
}
