using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class ReamSimpleHole : SimpleHole2
    {        
        public ReamSimpleHole(HolePackage hole) : base(hole)
        {
        }
        public override string GetProcessAbbrevate() => "REAM";

        public override string ToString()
        {
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            string result = Quantity > 1 ? $"{Quantity}-{GetProcessAbbrevate()} <o>{HoleDiameter:F2}H7 {depth}" :
                $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2}H7 {depth}";
            return result;
        }
    }
}
