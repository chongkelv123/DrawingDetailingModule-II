using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class CounterSunk : MyHoleFeature
    {
        public double CounterSunkDiameter { get; set; }
        public double CounterSunkAnlge { get; set; }
        public List<SymbolicThread> SymbolicThreads { get; set; }
        public CounterSunk(Feature feature) : base(feature)
        {
            SymbolicThreads = new List<SymbolicThread>();
        }
        public CounterSunk()
        {
            SymbolicThreads = new List<SymbolicThread>();
        }

        public override string GetProcessAbbrevate() => "C'SUNK";

        public override void GetFeatureDetailInformation(Feature feature)
        {
            HolePackage holePackage = feature as HolePackage;
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(holePackage);
            CounterSunkDiameter = hpBuilder.ScrewClearanceCountersinkDiameter.Value;
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            CounterSunkAnlge = hpBuilder.ScrewClearanceCountersinkAngle.Value;
            HoleDepth = hpBuilder.ScrewClearanceHoleDepth.Value;
            Quantity = points.Count;

            IsThruHole = AskThruHole(holePackage);

            Feature[] allChilds = holePackage.GetAllChildren();
            SymbolicThreads = GetSymbolicThread(allChilds, SymbolicThreads);
        }

        private bool AskThruHole(HolePackage hole)
        {
            int edit = 0;
            string diameter1;
            string diameter2;
            string depth1;
            string csink_anlge;
            string tip_angle;
            int thru_flag;
            TaggedObject taggedObject = NXOpen.Utilities.NXObjectManager.Get(hole.Tag);

            ufs.Modl.AskCSunkHoleParms(taggedObject.Tag, edit, out diameter1, out diameter2, out depth1, out csink_anlge, out tip_angle, out thru_flag);

            return thru_flag == 1;
        }

        public override string ToString()
        {
            string csunkCondition = GetHoleCondition(feature);
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            string description = "";
            if (csunkCondition == "")
            {
                description = $"{GetProcessAbbrevate()} <o>{CounterSunkDiameter:F1}x{CounterSunkAnlge}<$s>, " +
                $"{FeatureFactory.DR} <o>{HoleDiameter:F1} {depth}";
            }
            else
            {
                description = $"{GetProcessAbbrevate()} <o>{CounterSunkDiameter:F2}x{CounterSunkAnlge}<$s>, {csunkCondition}, " +
                $"{FeatureFactory.DR} <o>{HoleDiameter:F1} {depth}";
            }

            // Append the description
            if (SymbolicThreads.Count > 0)
            {                
                SymbolicThread x = SymbolicThreads[0];
                description += $", TAP M{x.MajorDiameter}x{x.Pitch} DP {x.Length:F1}";
            }

            return description;
        }
    }
}
