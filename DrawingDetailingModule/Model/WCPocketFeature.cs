using NXOpen;
using NXOpen.UF;
using NXOpen.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Model
{
    public class WCPocketFeature : MyFeature
    {
        Extrude extrureFeat;
        Feature sketchFeat;
        List<Point3d> wcspBasePoint;
        public UFSession ufs { get; set; }
        
        public TaggedObject SelectedBody { get; set; }

        Feature feature;
        public double WCStartPointDiamter { get; set; }

        public WCPocketFeature()
        {
        }

        public WCPocketFeature(Feature feature) : base(feature)
        {
            this.feature = feature;
            extrureFeat = feature as Extrude;
            sketchFeat = GetSketchFeat(feature);            
        }

        private Feature GetSketchFeat(Feature feature)
        {
            Feature result = null;
            foreach (NXObject ent in feature.GetParents())
            {
                if (ent is SketchFeature)
                {
                    return (SketchFeature)ent;
                }
                else if (ent is WaveSketch)
                {
                    return (WaveSketch)ent;
                }
            }
            return result;
        }

        public override void GetFeatureDetailInformation(Feature feature)
        {
            Quantity = 1;
        }

        public override string GetProcessAbbrevate() => FeatureFactory.WC;

        public List<Point3d> GenerateWCSPLocation()
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

        private Point3d calculateMidPoint(Line line)
        {
            double midX = (line.StartPoint.X + line.EndPoint.X) / 2;
            double midY = (line.StartPoint.Y + line.EndPoint.Y) / 2;
            double midZ = (line.StartPoint.Z + line.EndPoint.Z) / 2;

            return new Point3d(midX, midY, midZ);
        }

        private double CalculateLineLength(Line line)
        {
            double length = Math.Sqrt(
                Math.Pow(line.EndPoint.X - line.StartPoint.X, 2) +
                Math.Pow(line.EndPoint.Y - line.StartPoint.Y, 2) +
                Math.Pow(line.EndPoint.Z - line.StartPoint.Z, 2)
                );

            return length;
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
            string wcOffset = GetWCOffset(feature);
            WCStartPointDiamter = GetWCStartPointDiam(35.0);

            string description = $"PROF {GetProcessAbbrevate()} {wcOffset} {wcType} (<o>{WCStartPointDiamter:F1} {FeatureFactory.WC_SP})";

            return description;
        }

        public double GetWCStartPointDiam(double plateThickness)
        {
            if (plateThickness < 50.0)
            {
                return 3.0;
            }
            return 5.2;
        }
    }
}
