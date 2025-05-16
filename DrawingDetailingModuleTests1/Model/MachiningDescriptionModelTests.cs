using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingDetailingModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NXOpen;

namespace DrawingDetailingModule.Model.Tests
{
    [TestClass()]
    public class MachiningDescriptionModelTests
    {
        [TestMethod()]
        public void GetWCStartPointDiameterTest()
        {
            string input = "WC <o>6.00 H7 S/C (<o>3.0 wc sp), C'BORE <o>7.0 DP 20.0";
            MachiningDescriptionModel descriptionModel = new MachiningDescriptionModel();

            double result = descriptionModel.GetWCStartPointDiameter(input);

            Assert.AreEqual(3.0, result);
        }

        [TestMethod()]
        public void GetWCHoleDiameterTest()
        {
            string input = "WC <o>6.00 H7 S/C (<o>3.0 wc sp), C'BORE <o>7.0 DP 20.0";
            MachiningDescriptionModel descriptionModel = new MachiningDescriptionModel();

            double result = descriptionModel.GetWCHoleDiameter(input);

            Assert.AreEqual(6.00, result);
        }
        [TestMethod()]
        public void Constructor_DefaultConstructor_CreatesInstanceWithDefaultValues()
        {
            // Act
            var model = new MachiningDescriptionModel();

            // Assert
            Assert.IsNull(model.Description);
            Assert.AreEqual(0, model.Quantity);
            Assert.IsNull(model.Abbrevate);
            Assert.IsNull(model.Direction);
            Assert.IsNull(model.Points);
            Assert.IsNull(model.Height);
        }
        [TestMethod()]
        public void Constructor_ParameterizedConstructor_CreatesInstanceWithProvidedValues()
        {
            // Arrange
            string description = "Test Description";
            int quantity = 5;
            List<Point3d> points = new List<Point3d> { new Point3d(1, 2, 3) };
            string abbrevate = "TEST";
            double[] direction = new double[] { 0, 0, 1 };
            string height = "10.0";

            // Act
            var model = new MachiningDescriptionModel(description, quantity, points, abbrevate, direction, height);

            // Assert
            Assert.AreEqual(description, model.Description);
            Assert.AreEqual(quantity, model.Quantity);
            Assert.AreEqual(points, model.Points);
            Assert.AreEqual(abbrevate, model.Abbrevate);
            Assert.AreEqual(direction, model.Direction);
            Assert.AreEqual(height, model.Height);
        }
        [TestMethod()]
        public void IsDescriptionSame_EmptyList_ReturnsFalse()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>();
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description"
            };

            // Act
            bool result = MachiningDescriptionModel.IsDescriptionSame(models, targetModel);

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod()]
        public void IsDescriptionSame_NonMatchingDescriptions_ReturnsFalse()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Description 1" },
                new MachiningDescriptionModel { Description = "Description 2" }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description"
            };

            // Act
            bool result = MachiningDescriptionModel.IsDescriptionSame(models, targetModel);

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod()]
        public void IsDescriptionSame_MatchingDescriptions_ReturnsTrue()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Test Description" },
                new MachiningDescriptionModel { Description = "Description 2" }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description"
            };

            // Act
            bool result = MachiningDescriptionModel.IsDescriptionSame(models, targetModel);

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod()]
        public void IsDescriptionSame_MatchingDescriptionDifferentCase_ReturnsTrue()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Description 1" },
                new MachiningDescriptionModel { Description = "test description" }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "TEST DESCRIPTION"
            };

            // Act
            bool result = MachiningDescriptionModel.IsDescriptionSame(models, targetModel);

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod()]
        public void SumUpModelQuantity_EmptyList_DoesNotThrowException()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>();
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description",
                Quantity = 5
            };

            // Act & Assert
            try
            {
                MachiningDescriptionModel.SumUpModelQuantity(models, targetModel);
            }
            catch (Exception)
            {
                Assert.Fail("Exception should not be thrown.");
            }
        }
        [TestMethod()]
        public void SumUpModelQuantity_MatchingModel_QuantitiesAdded()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Description 1", Quantity = 3 },
                new MachiningDescriptionModel { Description = "Test Description", Quantity = 5 }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description",
                Quantity = 2
            };

            // Act
            MachiningDescriptionModel.SumUpModelQuantity(models, targetModel);

            // Assert
            Assert.AreEqual(7, models[1].Quantity); // 5 + 2 = 7
        }
        [TestMethod()]
        public void SumUpModelQuantity_NonMatchingModel_QuantitiesNotChanged()
        {
            // Arrange
            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Description 1", Quantity = 3 },
                new MachiningDescriptionModel { Description = "Test Description", Quantity = 5 }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Non-Matching Description",
                Quantity = 2
            };

            // Act
            MachiningDescriptionModel.SumUpModelQuantity(models, targetModel);

            // Assert
            Assert.AreEqual(3, models[0].Quantity);
            Assert.AreEqual(5, models[1].Quantity);
        }
        [TestMethod()]
        public void AppendModelPoints_MatchingModel_PointsAdded()
        {
            // Arrange
            var points1 = new List<Point3d> { new Point3d(1, 2, 3) };
            var points2 = new List<Point3d> { new Point3d(4, 5, 6), new Point3d(7, 8, 9) };
            var targetPoints = new List<Point3d> { new Point3d(10, 11, 12) };

            var models = new List<MachiningDescriptionModel>
            {
                new MachiningDescriptionModel { Description = "Description 1", Points = points1 },
                new MachiningDescriptionModel { Description = "Test Description", Points = points2 }
            };
            var targetModel = new MachiningDescriptionModel
            {
                Description = "Test Description",
                Points = targetPoints
            };

            // Act
            MachiningDescriptionModel.AppendModelPoints(models, targetModel);

            // Assert
            Assert.AreEqual(3, models[1].Points.Count); // 2 original + 1 added
            Assert.AreEqual(10, models[1].Points[2].X);
            Assert.AreEqual(11, models[1].Points[2].Y);
            Assert.AreEqual(12, models[1].Points[2].Z);
        }
        [TestMethod()]
        public void GetWCStartPointDiameter_ValidInput_ReturnsCorrectDiameter()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "PROF WC H7 S/C (<o>3.0 wc sp)";

            // Act
            double result = model.GetWCStartPointDiameter(input);

            // Assert
            Assert.AreEqual(3.0, result);
        }
        [TestMethod()]
        public void GetWCStartPointDiameter_ValidInputWithCapitalO_ReturnsCorrectDiameter()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "PROF WC H7 S/C (<O>5.2 wc sp)";

            // Act
            double result = model.GetWCStartPointDiameter(input);

            // Assert
            Assert.AreEqual(5.2, result);
        }
        [TestMethod()]
        public void GetWCStartPointDiameter_InvalidInput_ReturnsZero()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "PROF WC H7 S/C No Diameter";

            // Act
            double result = model.GetWCStartPointDiameter(input);

            // Assert
            Assert.AreEqual(0, result);
        }
        [TestMethod()]
        public void GetWCHoleDiameter_ValidInput_ReturnsCorrectDiameter()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "WC <o>10.5 H7 S/C, DR 5.0 THRU";

            // Act
            double result = model.GetWCHoleDiameter(input);

            // Assert
            Assert.AreEqual(10.5, result);
        }
        [TestMethod()]
        public void GetWCHoleDiameter_ValidInputWithCapitalO_ReturnsCorrectDiameter()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "WC <O>8.75 H7 S/C, DR 4.0 THRU";

            // Act
            double result = model.GetWCHoleDiameter(input);

            // Assert
            Assert.AreEqual(8.75, result);
        }
        [TestMethod()]
        public void GetWCHoleDiameter_InvalidInput_ReturnsZero()
        {
            // Arrange
            var model = new MachiningDescriptionModel();
            string input = "WC No Diameter Format";

            // Act
            double result = model.GetWCHoleDiameter(input);

            // Assert
            Assert.AreEqual(0, result);
        }

    }
}