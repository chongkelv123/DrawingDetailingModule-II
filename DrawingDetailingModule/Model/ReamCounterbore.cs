using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class ReamCounterbore : Counterbore2
    {
        public ReamCounterbore(Feature feature) : base(feature)
        {
        }

        public override string GetProcessAbbrevate() => "REAM";

        public override string ToString()
        {
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} H7 {depth}, " +                
                $"{FeatureFactory.CBORE} <o>{CounterboreDiamter:F1} {FeatureFactory.DP} {CounterDepth:F1}";
            string result = Quantity > 1 ? $"{Quantity}-{description}" :
                $"{description}";
            return result;
        }
    }


}
