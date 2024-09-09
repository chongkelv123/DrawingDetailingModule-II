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
    public class AskBoundingBoxTests
    {
        [TestMethod()]
        public void IsFromBackDirection_WhenYValueIsMaxInY_ReturnsTrue()
        {
            // Arrange        
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = 0;
            askBounding.MinZ = -125;
            askBounding.MaxX = 150;
            askBounding.MaxY = 550;
            askBounding.MaxZ = 0;

            // Act
            bool result = askBounding.IsFromBackDirection(550);

            // Assert
            Assert.AreEqual(true, result);
        }
        [TestMethod()]
        public void IsFromBackDirection_WhenYValueIsNotMaxInY_ReturnsFalse()
        {
            // Arrange        
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = 0;
            askBounding.MinZ = -125;
            askBounding.MaxX = 150;
            askBounding.MaxY = 550;
            askBounding.MaxZ = 0;

            // Act
            bool result = askBounding.IsFromBackDirection(0);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void IsFromFrontDirection_WhenYValueIsMinInY_ReturnTrue()
        {
            // Arrange        
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = 0;
            askBounding.MinZ = -125;
            askBounding.MaxX = 150;
            askBounding.MaxY = 550;
            askBounding.MaxZ = 0;

            // Act
            bool result = askBounding.IsFromFrontDirection(0);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void AskDirectionTest_ZvalueEqualMinZ_ReturnTrue()
        {
            // Arrange
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = -177.5;
            askBounding.MinZ = -35.0;
            askBounding.MaxX = 415;
            askBounding.MaxY = 177.5;
            askBounding.MaxZ = 0;

            // Act
            double[] point = new double[3] { 0, 0, -35 };
            double[] result = askBounding.AskDirection(point, AXIS.Z);
            double[] expected = new double[3] { 0, 0, 1 };

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void AskDirectionTest_ZvalueEqualMaxZ_ReturnTrue()
        {
            // Arrange
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = -177.5;
            askBounding.MinZ = -35.0;
            askBounding.MaxX = 415;
            askBounding.MaxY = 177.5;
            askBounding.MaxZ = 0;

            // Act
            double[] point = new double[3] { 0, 0, 0 };
            double[] result = askBounding.AskDirection(point, AXIS.Z);
            double[] expected = new double[3] { 0, 0, -1 };

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
        
        [TestMethod()]
        public void AskDirectionTest_ZvalueInTheMiddle_ReturnTrue()
        {
            // Arrange
            AskBoundingBox askBounding = new AskBoundingBox();
            askBounding.MinX = 0;
            askBounding.MinY = -177.5;
            askBounding.MinZ = -35.0;
            askBounding.MaxX = 415;
            askBounding.MaxY = 177.5;
            askBounding.MaxZ = 0;

            // Act
            double[] point = new double[3] { 0, 0, 10 };
            double[] result = askBounding.AskDirection(point, AXIS.Z);
            double[] expected = new double[3] { 0, 0, -1 };

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}