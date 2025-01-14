using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;
using NXOpen.UF;

namespace DrawingDetailingModule.Model
{
    public class WCCounterbore : Counterbore2, IMyWCFeature
    {
        Feature feature;
        public double WCStartPointDiamter { get; set; }

        public WCCounterbore(Feature feature) : base(feature)
        {
            this.feature = feature;
        }
        public override string GetProcessAbbrevate() => FeatureFactory.WC;

        public override string ToString()
        {
            string wcType = GetWCCondition(feature);
            wcType = ProcessWCType(wcType);
            string wcOffset = GetWCOffset(feature);
            WCStartPointDiamter = GetWCHoleSize(HoleDiameter);

            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} {wcOffset} {wcType} (<o>{WCStartPointDiamter:F1} {FeatureFactory.WC_SP}), " +
                $"{FeatureFactory.CBORE} <o>{CounterboreDiamter:F1} {FeatureFactory.DP} {CounterDepth:F1}";

            // Append the description
            if (SymbolicThreads.Count > 0)
            {
                foreach (SymbolicThread t in SymbolicThreads)
                {
                    description += $", TAP M{t.MajorDiameter}x{t.Pitch} DP {t.Length:F1}";
                }

            }

            return description;
        }

    }
}
