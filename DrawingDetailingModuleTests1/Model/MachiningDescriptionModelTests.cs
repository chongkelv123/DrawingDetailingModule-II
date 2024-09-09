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
    public class MachiningDescriptionModelTests
    {
        [TestMethod()]
        public void GetWCStartPointDiameterTest()
        {
            string input = "WC <o>6.00 H7 S/C (<o>3.0 wc sp), C'BORE <o>7.0 DP 20.0";
;
            MachiningDescriptionModel descriptionModel = new MachiningDescriptionModel();

            double result = descriptionModel.GetWCStartPointDiameter(input);

            Assert.AreEqual(3.0, result);
        }
    }
}