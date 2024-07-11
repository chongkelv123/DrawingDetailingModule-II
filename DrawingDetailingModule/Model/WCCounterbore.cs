using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class WCCounterbore : Counterbore2
    {
        Feature feature;

        public WCCounterbore(Feature feature) : base(feature)
        {
            this.feature = feature;
        }
        public override string GetProcessAbbrevate() => "WC";

        public override string ToString()
        {
            string wcType = GetWCCondition(feature);
            string wcOffset = GetWCOffset(feature);
            double wcholeDiameter = GetWCHoleSize(HoleDiameter);

            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} {wcOffset} {wcType} ({wcholeDiameter} {FeatureFactory.WC_SP}), " +
                $"{FeatureFactory.CBORE} <o>{CounterboreDiamter:F1} {FeatureFactory.DP} {CounterDepth:F1}";           

            return description;
        }
    }
}
