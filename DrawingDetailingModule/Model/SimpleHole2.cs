using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;
using NXOpen.UF;

namespace DrawingDetailingModule.Model
{
    public class SimpleHole2 : MyHoleFeature
    {
        public SimpleHole2(HolePackage hole) : base (hole)
        {
        }
        public override string GetProcessAbbrevate() => "DR";
        
        public override void GetFeatureDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            HoleDepth = hpBuilder.ScrewClearanceHoleDepth.Value;
            Quantity = points.Count;

            IsThruHole = AskThruHole(hole);
        }

        private bool AskThruHole(HolePackage hole)
        {
            int edit = 0;
            string diameter;
            string depth;
            string tip_angle;
            int thru_flag;
            TaggedObject taggedObject = NXOpen.Utilities.NXObjectManager.Get(hole.Tag);
            ufs.Modl.AskSimpleHoleParms(taggedObject.Tag, edit, out diameter, out depth, out tip_angle, out thru_flag);

            return thru_flag == 1;
        }

        public override string ToString()
        {
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            string result = $"{GetProcessAbbrevate()} <o>{HoleDiameter:F1} {depth}";

            return result;
        }

    }
}
