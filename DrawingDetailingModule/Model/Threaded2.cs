using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;
using NXOpen.Routing.Electrical;
using NXOpen.UF;
using static NXOpen.Drawings.CustomViewSettingsBuilder;

namespace DrawingDetailingModule.Model
{
    public class Threaded2 : MyHoleFeature
    {
        public string ThreadSize { get; set; }
        public double ThreadDepth { get; set; }
        public double TapDrill { get; set; }
        public string ThreadStandard { get; set; }
        bool isThruThread = false;
        bool isThruHole = false;
        const string METRIC_FINE = "Metric Fine";
        const string METRIC_COARSE = "Metric Coarse";
        public Threaded2(HolePackage hole) : base(hole)
        {
        }

        public override string ToString()
        {
            string cutCondition = GetTAPECondition(feature);
            string depth = isThruThread ? "THRU" : $"DP {ThreadDepth:F1}";
            string result = "";

            if (ThreadStandard.Equals(METRIC_COARSE))
            {
                result = $"{GetProcessAbbrevate()} {ThreadSize} {depth}";
            }
            else if (ThreadStandard.Equals(METRIC_FINE))
            {
                result = $"{GetProcessAbbrevate()} {ThreadSize} {depth}, DR <O>{TapDrill} THRU";
            }

            if (!cutCondition.Equals(""))
            {
                result = result + " " + cutCondition;
            }

            return result;
        }

        public override string GetProcessAbbrevate() => "TAP";


        public override void GetFeatureDetailInformation(Feature feature)
        {            
            HolePackage holePackage = feature as HolePackage;
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(holePackage);
            ThreadSize = hpBuilder.ThreadSize;
            ThreadStandard = hpBuilder.ThreadStandard;
            TapDrill = hpBuilder.TapDrillDiameter.Value;
            var option = hpBuilder.HoleDepthLimitOption;
            if (option == HolePackageBuilder.HoleDepthLimitOptions.ThroughBody && ThreadStandard.Equals(METRIC_COARSE))
            {
                isThruThread = true;
            }
            else if (option == HolePackageBuilder.HoleDepthLimitOptions.ThroughBody && !ThreadStandard.Equals(METRIC_FINE))
            {
                isThruThread = false;
                isThruHole = true;
            }
            ThreadDepth = hpBuilder.ThreadDepth.Value;
            Quantity = points.Count;
        }
    }
}
