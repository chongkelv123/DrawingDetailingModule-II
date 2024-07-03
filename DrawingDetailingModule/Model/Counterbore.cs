using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class Counterbore : SimpleHole
    {                              
        protected double CounterboreDiamter { get; set; }
        protected double CounterDepth { get; set; }

        const string PROCESS_ABBREVATE = "C'BORE";

        public Counterbore(HolePackage hole) : base(hole)
        {                                        
            //GetHoleDetailInformation(hole);
        }

        public new void GetHoleDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            CounterboreDiamter = hpBuilder.ScrewClearanceCounterboreDiameter.Value;
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            CounterDepth = hpBuilder.ScrewClearanceCounterboreDepth.Value;
            Quantity = points.Count;
            TaggedObject taggedObject = NXOpen.Utilities.NXObjectManager.Get(hole.Tag);            
            IsThruHole = AskThruHole(hole);
        }

        private bool AskThruHole(HolePackage hole)
        {            
            int edit = 0;
            string diameter1;
            string diameter2;
            string depth1;
            string depth2;
            string tip_angle;
            int thru_flag;
            TaggedObject taggedObject = NXOpen.Utilities.NXObjectManager.Get(hole.Tag);
            ufs.Modl.AskCBoreHoleParms(taggedObject.Tag, edit, out diameter1, out diameter2, out depth1, out depth2, out tip_angle, out thru_flag);

            return thru_flag == 1;
        }

        public override string ToString()
        {
            string depth = IsThruHole ? "THRU" : $"{HoleDepth:F1}";
            return $"{Quantity}-{PROCESS_ABBREVATE} <O>{CounterboreDiamter:F1} DP {CounterDepth:F1},\n"+
                $"DR <o>{HoleDiameter:F1} {depth}";                
        }
    }
}
