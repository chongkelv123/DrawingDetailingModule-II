using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;
using NXOpen.UF;

namespace DrawingDetailingModule.Model
{
    public class Threaded2 : MyFeature
    {        
        public string ThreadSide { get; set; }
        public double ThreadDepth { get; set; }        
        public Threaded2(HolePackage hole):base(hole)
        {           
        }              

        public override string ToString()
        {
            string result = $"{GetProcessAbbrevate()} {ThreadSide} DP {ThreadDepth:F1}";

            return result;
        }

        public override string GetProcessAbbrevate() => "TAP";
        

        public override void GetFeatureDetailInformation(HolePackage holePackage)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(holePackage);
            ThreadSide = hpBuilder.ThreadSize;
            ThreadDepth = hpBuilder.ThreadDepth.Value;
            Quantity = points.Count;
        }
    }
}
