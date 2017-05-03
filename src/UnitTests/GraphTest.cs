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
    }
}
