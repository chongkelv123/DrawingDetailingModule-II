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
    public class Threaded2 : MyHoleFeature
    {
        public string ThreadSide { get; set; }
        public double ThreadDepth { get; set; }
        bool isThruThread = false;
        public Threaded2(HolePackage hole) : base(hole)
        {
        }

        public override string ToString()
        {
            string depth = isThruThread ? "THRU" : $"DP {ThreadDepth:F1}";
            string result = $"{GetProcessAbbrevate()} {ThreadSide} {depth}";

            return result;
        }

        public override string GetProcessAbbrevate() => "TAP";


        public override void GetFeatureDetailInformation(Feature feature)
        {            
            HolePackage holePackage = feature as HolePackage;
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(holePackage);
            ThreadSide = hpBuilder.ThreadSize;
            var option = hpBuilder.HoleDepthLimitOption;
            if (option == HolePackageBuilder.HoleDepthLimitOptions.ThroughBody)
            {
                isThruThread = true;
            }
            ThreadDepth = hpBuilder.ThreadDepth.Value;
            Quantity = points.Count;
        }
    }
}
