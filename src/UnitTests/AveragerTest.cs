using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows;
using MidSurfaceNameSpace.Solver;

namespace UnitTests
{
    [TestClass]
    public class AveragingTest1
    {
        [TestMethod]
        public void TestInit()
        {
            var pth = new List<List<Point>>();

            var p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(3, 1));
            pth.Add(p);

            p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(1, 3));
            pth.Add(p);
            Averager averager = new Averager();
            averager.Init(pth);
            Assert.AreEqual(Math.Round(averager.LinearPaths[0][0].K, 2), 0.33);
            
        }
        [TestMethod]
        public void TestGetPointByDistanceSuperSimple()
        {
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(2, 0));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0][0];
            var result = testPath.GetPointByDistance(1); 

            Assert.AreEqual(result.X,1);
            Assert.AreEqual(result.Y, 0);
        }

        [TestMethod]
        public void TestGetPointByDistanceSimple()
        {
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(-2, -2));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0][0];
            var result = testPath.GetPointByDistance(1.414213562373095);

            Assert.AreEqual(Math.Round(result.X,1), -1.0);
            Assert.AreEqual(Math.Round(result.Y,1), -1.0);
        }
        [TestMethod]
        public void TestGetPointByDistanceSimple2()
        {
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(2, 4));
            p.Add(new Point(0, 0));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0][0];
            var result = testPath.GetPointByDistance(2.23606797749979);

            Assert.AreEqual(Math.Round(result.X, 1), 1);
            Assert.AreEqual(Math.Round(result.Y, 1), 2);
        }
        [TestMethod]
        public void TestGetPointByDistanceSimple3()
        {
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(-2, 4));
            p.Add(new Point(0, 0));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0][0];
            var result = testPath.GetPointByDistance(2.23606797749979);

            Assert.AreEqual(Math.Round(result.X, 1), -1);
            Assert.AreEqual(Math.Round(result.Y, 1), 2);
        }

        [TestMethod]
        public void TestMassiveGetPointByDistanceSimple()
        {

            //  тестовая фигура выглядит так:           /
            //                                      ___/
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(-4, 0));
            p.Add(new Point(0, 0));
            p.Add(new Point(2, 4));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0];
            
            var result = averager.GetPointByDistance(testPath,4);

            Assert.AreEqual(Math.Round(result.X, 1), 0);
            Assert.AreEqual(Math.Round(result.Y, 1), 0);
        }
        [TestMethod]
        public void TestMassiveGetPointByDistanceSimple2()
        {
            
            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(2, -4));
            p.Add(new Point(0, 0));
            p.Add(new Point(2, 4));
            pth.Add(p);

            Averager averager = new Averager();
            averager.Init(pth);

            var testPath = averager.LinearPaths[0];

            var result = averager.GetPointByDistance(testPath, 8.944271909999159);

            Assert.AreEqual(Math.Round(result.X, 1), 2);
            Assert.AreEqual(Math.Round(result.Y, 1), 4);
        }
        
        [TestMethod]
        public void TestGridPath()
        {

            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(4, 0));
            pth.Add(p);
            
            Averager averager = new Averager();
            averager.Init(pth);
            averager.Accuracy = 4;
            var testPath = averager.LinearPaths[0];

            var result = averager.GridPath(testPath);

            Assert.AreEqual(result.Count,4);
            
        }

        [TestMethod]
        public void TestGetAveragePath()
        {

            var pth = new List<List<Point>>();
            var p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(4, 0));
            pth.Add(p);

            var p2 = new List<Point>();
            p2.Add(new Point(0, 0));
            p2.Add(new Point(0, 4));
            pth.Add(p2);

            Averager averager = new Averager();
            averager.Accuracy = 4;
            
            var result = averager.GetAveragePath(pth);

            //Assert.AreEqual(result.Count, 4);

        }

    }
}
