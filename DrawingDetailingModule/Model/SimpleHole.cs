using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.Features;
using NXOpen.UF;

namespace DrawingDetailingModule.Model
{
    public class SimpleHole
    {
        protected Part workPart;
        protected UFSession ufs;
        protected HashSet<Point2d> points;

        public double HoleDiameter { get; set; }
        public double HoleDepth { get; set; }
        public int Quantity { get; set; }
        public bool IsThruHole {get; set;}

        const string PROCESS_ABBREVATE = "DR";

        public SimpleHole(HolePackage hole)
        {
            workPart = Session.GetSession().Parts.Work;
            points = new HashSet<Point2d>();
            ufs = UFSession.GetUFSession();

            GetPointsFromEdges(hole);
            //GetHoleDetailInformation(hole);
        }

        public void GetPointsFromEdges(HolePackage hole)
        {
            Edge[] edges = hole.GetEdges();
            var circularEdges = edges
                .Where(edge => edge.SolidEdgeType == Edge.EdgeType.Circular)
                .Select(edge => edge.GetLocations()[0].Location);

            circularEdges.ToList().ForEach(x => points.Add(new Point2d(x.X, x.Y)));
        }

        public void GetFeatureDetailInformation(HolePackage hole)
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
            string result = Quantity > 1 ? $"{Quantity}-{PROCESS_ABBREVATE} <o>{HoleDiameter:F1} {depth}" :
                $"{PROCESS_ABBREVATE} <o>{HoleDiameter:F1} {depth}";
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
