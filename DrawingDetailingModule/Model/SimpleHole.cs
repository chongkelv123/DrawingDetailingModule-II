using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class SimpleHole
    {
        protected Part workPart;
        protected HashSet<Point2d> points;

        public double HoleDiameter { get; set; }
        public double HoleDepth { get; set; }
        public int Quantity { get; set; }

        const string PROCESS_ABBREVATE = "DR";

        public SimpleHole(HolePackage hole)
        {
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();

            GetPointsFromEdges(hole);
            GetHoleDetailInformation(hole);
        }

        public void GetPointsFromEdges(HolePackage hole)
        {
            Edge[] edges = hole.GetEdges();
            var circularEdges = edges
                .Where(edge => edge.SolidEdgeType == Edge.EdgeType.Circular)
                .Select(edge => edge.GetLocations()[0].Location);

            circularEdges.ToList().ForEach(x => points.Add(new Point2d(x.X, x.Y)));
        }

        public void GetHoleDetailInformation(HolePackage hole)
        {            
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            HoleDepth = hpBuilder.ScrewClearanceHoleDepth.Value;
            Quantity = points.Count;
        }

        public override string ToString()
        {
            string result = Quantity > 1 ? $"{Quantity}-{PROCESS_ABBREVATE} <o>{HoleDiameter:F1} THRU" :
                $"{PROCESS_ABBREVATE} <o>{HoleDiameter:F1} THRU";
            return result;
        }

        public string ToString(List<Point2d> points)
        {
            StringBuilder stringBuilder = new StringBuilder();
            points.ForEach(x => stringBuilder.Append($"{x.X:F3},{x.Y:F3}\n"));
            return stringBuilder.ToString();
        }

        public List<Point2d> GetLocation()
        {
            return points.ToList();
        }
    }
}
