using NXOpen.Features;
using NXOpen;
using NXOpen.UF;
using NXOpen.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.CAE.Optimization;
using NXOpen.GeometricUtilities;

namespace DrawingDetailingModule.Model
{
    public class MillPocketFeature : MyPocketFeature
    {
        public MillPocketFeature(Feature feature) : base(feature)
        {
            IsThru = false;
        }

        public double Depth { get; set; }
        public bool IsThru { get; set; }

        public override string GetProcessAbbrevate() => FeatureFactory.MILL;

        public override List<Point3d> GenerateLocation()
        {
            List<Point3d> points = new List<Point3d>();

            if (sketchFeat == null)
            {
                return points;
            }

            GetBoundingBoxInfo getBounding = new GetBoundingBoxInfo(sketchFeat);
            Point3d midPoint = getBounding.MidPoint;

            return new List<Point3d>() { midPoint };
        }

        public override void GetFeatureDetailInformation(Feature feature)
        {            
            ExtrudeBuilder extrudeBuilder = workPart.Features.CreateExtrudeBuilder(feature);
            var trimType = extrudeBuilder.Limits.EndExtend.TrimType;
            if (trimType is NXOpen.GeometricUtilities.Extend.ExtendType.ThroughAll)
            {
                IsThru = true;
            }else if(trimType is Extend.ExtendType.Value)
            {
                Depth = extrudeBuilder.Limits.EndExtend.Value.Value;
            }
            Quantity = 1;
        }

        public override string ToString()
        {
            string millCondition = GetMILLCondition(feature);
            string millDepth = IsThru ? "THRU" : $"DP {Depth.ToString()}";
            string description = $"PROF {GetProcessAbbrevate()} {millCondition} {millDepth}";

            return description;
        }
    }
}
