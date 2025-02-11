using NXOpen.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Model
{
    public class MillSimpleHole:SimpleHole2
    {
        public MillSimpleHole(HolePackage hole) : base(hole) {}

        public override string GetProcessAbbrevate() => "MILL";

        public override string ToString()
        {
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            string description = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F2} {depth}";

            return description;
        }
    }
}
