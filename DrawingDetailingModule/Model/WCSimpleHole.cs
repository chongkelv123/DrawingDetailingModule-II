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
        Feature feature;
        public WCSimpleHole(HolePackage hole) : base(hole)
        {
            feature = hole;
        }
        public override string GetProcessAbbrevate() => "WC";

        public override string ToString()
        {            
            string wcType = GetWCCondition(feature);            
            string wcOffset = GetWCOffset(feature);
            double wcholeDiameter  = GetWCHoleSize(HoleDiameter);

            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} {wcOffset} {wcType},\n" +
                $"(<O>{wcholeDiameter} wc sp)";

            string result = Quantity > 1 ? $"{Quantity}-{description}" :
                $"{description}";

            return result;
        }

        
    }


}
