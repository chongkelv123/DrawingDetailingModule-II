using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class Threaded : SimpleHole
    {
        public string ThreadSide { get; set; }
        public double ThreadDepth { get; set; }
        const string PROCESS_ABBREVATE = "TAP";
        public Threaded(HolePackage hole) : base(hole)
        {
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();

            GetPointsFromEdges(hole);
            GetHoleDetailInformation(hole);
        }

        private new void GetHoleDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            ThreadSide = hpBuilder.ThreadSize;
            ThreadDepth = hpBuilder.ThreadDepth.Value;
            Quantity = points.Count;
        }

        public override string ToString()
        {
            string result = Quantity > 1? $"{Quantity}-{PROCESS_ABBREVATE} {ThreadSide} DP {ThreadDepth:F1}" : 
                $"{PROCESS_ABBREVATE} {ThreadSide} DP {ThreadDepth:F1}";
            return result;
        }

        
    }
}
