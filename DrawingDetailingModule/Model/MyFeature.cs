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
    public abstract class MyFeature : IFeature
    {
        protected Part workPart;
        protected UFSession ufs;
        protected HashSet<Point2d> points;

        public int Quantity { get; set; }

        public MyFeature(HolePackage hole)
        {
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();
            ufs = UFSession.GetUFSession();

            GetPointsFromEdges(hole);
        }

        public abstract void GetFeatureDetailInformation(HolePackage holePackage);

        public abstract string GetProcessAbbrevate();        

        public void GetPointsFromEdges(HolePackage hole)
        {
            Edge[] edges = hole.GetEdges();
            var circularEdges = edges
                .Where(edge => edge.SolidEdgeType == Edge.EdgeType.Circular)
                .Select(edge => edge.GetLocations()[0].Location);

            circularEdges.ToList().ForEach(x => points.Add(new Point2d(x.X, x.Y)));
        }

        public string GetProcessType(HolePackage holePackage)
        {
            string input = holePackage.JournalIdentifier;
            var match = Regex.Match(input, @"^(\w+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }

        public List<Point2d> GetLocation()
        {
            return points.ToList();
        }

        public string ToString(List<Point2d> points)
        {
            StringBuilder stringBuilder = new StringBuilder();
            points.ForEach(x => stringBuilder.Append($"{x.X:F3},{x.Y:F3}\n"));
            return stringBuilder.ToString();
        }
    }
}
