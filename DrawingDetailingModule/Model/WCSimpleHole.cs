using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class WCSimpleHole : SimpleHole2
    {
        public WCSimpleHole(HolePackage hole) : base(hole)
        {
        }
        public override string GetProcessAbbrevate() => "WC";

        public override string ToString()
        {
            string depth = "THRU";
            string result = Quantity > 1 ? $"{Quantity}-{GetProcessAbbrevate()} <o>{HoleDiameter:F2}H7 {depth}" :
                $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2}H7 {depth}";
            return result;
        }
    }


}
