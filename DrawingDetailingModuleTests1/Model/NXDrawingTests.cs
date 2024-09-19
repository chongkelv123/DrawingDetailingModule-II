using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingDetailingModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Model.Tests
{
    [TestClass()]
    public class NXDrawingTests
    {
        [TestMethod()]
        public void NumberToAlphabetTest()
        {
            // Arrange
            NXDrawing xDrawing = new NXDrawing();
            int number = 600;

            // Act
            string result = xDrawing.NumberToAlphabet(number);

            // Assert
            Assert.AreEqual("AAA", result);
        }
    }
}