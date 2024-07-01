using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class Hole
    {
        Part workPart;
        HashSet<Point2d> points;

        public double HoleDiameter { get; set;}
        public int Quantity { get; set; }

        public Hole(HolePackage hole)
        {
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();

            GetPointsFromEdges(hole);
            GetHoleDetailInformation(hole);
        }

        private void GetPointsFromEdges(HolePackage hole)
        {
            Edge[] edges = hole.GetEdges();
            var circularEdges = edges
                .Where(edge => edge.SolidEdgeType == Edge.EdgeType.Circular)
                .Select(edge => edge.GetLocations()[0].Location);

            circularEdges.ToList().ForEach(x => points.Add(new Point2d(x.X, x.Y)));
        }

        private void GetHoleDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);            
            HoleDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;            
            Quantity = points.Count;
        }

        public override string ToString()
        {
            return 
                $"holeDiameter: {HoleDiameter}\n" +                
                $"quantity: {Quantity}\n";
        }
    }
}
