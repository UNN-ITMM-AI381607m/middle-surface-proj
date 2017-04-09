using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.IO;
using MidSurfaceNameSpace.Primitive;
using System.Diagnostics;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestImportMethodCorrect()
        {
            Parser parser = new Parser();
            string pathToTestFile = Environment.CurrentDirectory + @"\test1.xml";

            //Try to parse test xml
            Figure figure = parser.ImportFile(pathToTestFile);

            //Check that we got figure
            Assert.AreNotEqual(null, figure);
        }
    }
}
