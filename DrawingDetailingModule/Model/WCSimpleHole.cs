using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class WCSimpleHole : SimpleHole2, IMyWCFeature
    {
        Feature feature;

        public double WCStartPointDiamter { get; set; }

        public WCSimpleHole(HolePackage hole) : base(hole)
        {
            feature = hole;
        }
        public override string GetProcessAbbrevate() => "WC";

        public override string ToString()
        {
            string wcType = GetWCCondition(feature);
            string wcOffset = GetWCOffset(feature);
            WCStartPointDiamter = GetWCHoleSize(HoleDiameter);

            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} {wcOffset} {wcType}, " +
                $"(<o>{WCStartPointDiamter:F1} {FeatureFactory.WC_SP})";

            return description;
        }


    }


}
