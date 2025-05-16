using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingDetailingModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

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
        public void MachiningDescriptionModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MachiningDescriptionModelTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsDescriptionSameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SumUpModelQuantityTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AppendModelPointsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetWCStartPointDiameterTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetWCHoleDiameterTest1()
        {
            Assert.Fail();
        }
    }
}