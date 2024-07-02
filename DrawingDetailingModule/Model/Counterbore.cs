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
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();          

            GetPointsFromEdges(hole);
            GetHoleDetailInformation(hole);
        }

        private new void GetHoleDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            CounterboreDiamter = hpBuilder.ScrewClearanceCounterboreDiameter.Value;
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            CounterDepth = hpBuilder.ScrewClearanceCounterboreDepth.Value;
            Quantity = points.Count;
        }

        public override string ToString()
        {
            return $"{Quantity}-{PROCESS_ABBREVATE} <O>{CounterboreDiamter:F1} DP {CounterDepth:F1},\n"+
                $"DR <o>{HoleDiameter:F1} THRU";
                
        }
    }
}
