using NXOpen;
using NXOpen.UF;
using NXOpen.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.GeometricUtilities;

namespace DrawingDetailingModule.Model
{
    public class WCPocketFeature : MyPocketFeature
    {        
        List<Point3d> wcspBasePoint;
        
        public double WCStartPointDiameter { get; set; }
        public double Depth { get; set; }
        public bool IsThru { get; set; }

        public WCPocketFeature(Feature feature) : base(feature) {}              

        public override string GetProcessAbbrevate() => FeatureFactory.WC;

        public override List<Point3d> GenerateLocation()
        {
            List<Point3d> points = new List<Point3d>();

            if (sketchFeat == null)
            {
                return points;
            }

            Line longestLine = null;
            double maxLength = 0.0;

            foreach (NXObject ent in sketchFeat.GetEntities())
            {
                if (ent is Line)
                {
                    Line line = (Line)ent;
                    double length = CalculateLineLength(line);

                    if (length > maxLength)
                    {
                        maxLength = length;
                        longestLine = line;
                    }
                }
            }

            if (longestLine != null)
            {                
                Point3d midPoint = calculateMidPoint(longestLine);
                bool flip = true;
                Point3d offsetPoint = OffsetPerpendicular(longestLine, midPoint, 3.0, flip);
                AskBoundingBox askBounding = new AskBoundingBox(ufs, SelectedBody.Tag);                
                flip = askBounding.IsPointContainInBoundary(offsetPoint, SelectedBody.Tag);
                if (flip)
                {
                    offsetPoint = OffsetPerpendicular(longestLine, midPoint, 3.0, !flip);
                }
                points.Add(offsetPoint);
            }

            wcspBasePoint = points;

            return points;
        }        

        public static Point3d OffsetPerpendicular(Line line, Point3d midPoint, double offsetDistance, bool flip)
        {
            // Calculate the direction of the line (normalized vector)
            Vector3d lineDirection = new Vector3d(
                line.EndPoint.X - line.StartPoint.X,
                line.EndPoint.Y - line.StartPoint.Y,
                line.EndPoint.Z - line.StartPoint.Z
            );

            // Normalize the line direction vector
            double length = Math.Sqrt(lineDirection.X * lineDirection.X +
                                      lineDirection.Y * lineDirection.Y +
                                      lineDirection.Z * lineDirection.Z);

            lineDirection.X /= length;
            lineDirection.Y /= length;
            lineDirection.Z /= length;

            // Calculate a vector perpendicular to the line in 2D (XY plane)
            Vector3d perpendicularDirection = new Vector3d(-lineDirection.Y, lineDirection.X, 0);

            Point3d offsetPoint;
            if (flip)
            {
                // Offset the midpoint by the perpendicular vector
                offsetPoint = new Point3d(
                    midPoint.X - perpendicularDirection.X * offsetDistance,
                    midPoint.Y - perpendicularDirection.Y * offsetDistance,
                    midPoint.Z  // Z remains the same (2D sketch assumption)
                );
            }
            else
            {
                offsetPoint = new Point3d(
                    midPoint.X + perpendicularDirection.X * offsetDistance,
                    midPoint.Y + perpendicularDirection.Y * offsetDistance,
                    midPoint.Z  // Z remains the same (2D sketch assumption)
                );
            }

            return offsetPoint;
        }

        public override string ToString()
        {
            string wcType = GetWCCondition(feature);
            wcType = ProcessWCType(wcType);
            string wcOffset = GetWCOffset(feature);
            WCStartPointDiameter = GetWCStartPointDiam(35.0);

            string description = $"PROF {GetProcessAbbrevate()} {wcOffset} {wcType} (<o>{WCStartPointDiameter:F1} {FeatureFactory.WC_SP})";

            return description;
        }

        private string ProcessWCType(string wcType)
        {
            if(!wcType.Equals("T/C", StringComparison.OrdinalIgnoreCase))
            {
                return wcType;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(wcType);
            sb.Append($" (L={Depth}, T=1<$s>)");
            return sb.ToString();
        }

        public double GetWCStartPointDiam(double plateThickness)
        {
            if (plateThickness < 50.0)
            {
                return 3.0;
            }
            return 5.2;
        }

        public override void GetFeatureDetailInformation(Feature feature)
        {
            ExtrudeBuilder extrudeBuilder = workPart.Features.CreateExtrudeBuilder(feature);
            var trimType = extrudeBuilder.Limits.EndExtend.TrimType;
            if (trimType is NXOpen.GeometricUtilities.Extend.ExtendType.ThroughAll)
            {
                IsThru = true;
            }
            else if (trimType is Extend.ExtendType.Value)
            {
                Depth = extrudeBuilder.Limits.EndExtend.Value.Value;
            }
            Quantity = 1;
        }
    }
}
