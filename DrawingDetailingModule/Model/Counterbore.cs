using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class Counterbore
    {        
        Part workPart;
        HashSet<Point2d> points;
        
        double counterboreDiamter;
        double counterDepth;

        double holeDiameter;        
        int quantity;

        public Counterbore(HolePackage hole) 
        {            
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();          

            GetPointsFromEdges(hole);
            GetHoleDetailInformation(hole);
        }

        private void GetHoleDetailInformation(HolePackage hole)
        {
            HolePackageBuilder hpBuilder = workPart.Features.CreateHolePackageBuilder(hole);
            counterboreDiamter = hpBuilder.ScrewClearanceCounterboreDiameter.Value;
            holeDiameter = hpBuilder.ScrewClearanceHoleDiameter.Value;
            counterDepth = hpBuilder.ScrewClearanceCounterboreDepth.Value;
            quantity = points.Count;
        }

        private void GetPointsFromEdges(HolePackage hole)
        {
            Edge[] edges = hole.GetEdges();
            var circularEdges = edges
                .Where(edge => edge.SolidEdgeType == Edge.EdgeType.Circular)
                .Select(edge => edge.GetLocations()[0].Location);

            circularEdges.ToList().ForEach(x => points.Add(new Point2d(x.X, x.Y)));
        }

        public override string ToString()
        {
            return $"counterboreDiamter: {counterboreDiamter}\n" +
                $"holeDiameter: {holeDiameter}\n" +
                $"counterDepth: {counterDepth}\n" +
                $"quantity: {quantity}\n";
        }
    }
}
