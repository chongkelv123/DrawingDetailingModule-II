using NXOpen;
using NXOpen.Annotations;
using NXOpen.Diagramming.Geometry;
using NXOpen.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Model
{
    public class BoundingBox
    {
        public Point3d MaxPoint { get; set; }
        public Point3d MinPoint { get; set; }
        public Point3d MidPoint { get; set; }
        Feature feature;
        List<Point3d> pointCollection;
        public BoundingBox(Feature feature)
        {
            this.feature = feature;
            pointCollection = new List<Point3d>();
            process();
        }

        private void process()
        {
            if (feature == null)
            {
                return;
            }

            foreach (NXObject ent in feature.GetEntities())
            {
                if (ent is NXOpen.Arc)
                {
                    NXOpen.Arc arc = (NXOpen.Arc)ent;
                    double radius = arc.Radius;
                    Point3d centerPt = arc.CenterPoint;
                    pointCollection.Add(centerPt);
                    pointCollection.Add(new Point3d(centerPt.X + radius, centerPt.Y, centerPt.Z));
                    pointCollection.Add(new Point3d(centerPt.X - radius, centerPt.Y, centerPt.Z));
                    pointCollection.Add(new Point3d(centerPt.X, centerPt.Y + radius, centerPt.Z));
                    pointCollection.Add(new Point3d(centerPt.X, centerPt.Y - radius, centerPt.Z));

                }
                else if (ent is NXOpen.Line)
                {
                    NXOpen.Line line = (NXOpen.Line)ent;
                    Point3d startPt = line.StartPoint;
                    Point3d endPt = line.EndPoint;
                    pointCollection.Add(startPt);
                    pointCollection.Add(endPt);
                }
            }

            MinPoint = CalculateMinPoint();

            MaxPoint = CalculateMaxPoint();

            MidPoint = CalculateMidPoint(MinPoint, MaxPoint);
        }

        private Point3d CalculateMidPoint(Point3d minpt, Point3d maxpt)
        {
            double midX = (minpt.X + maxpt.X) / 2;
            double midY = (minpt.Y + maxpt.Y) / 2;
            double midZ = (minpt.Z + maxpt.Z) / 2;

            return new Point3d(midX, midY, midZ);
        }

        private Point3d CalculateMinPoint()
        {
            Point3d minXPoint = pointCollection.Aggregate((min, cur) => cur.X < min.X ? cur : min);
            Point3d minYPoint = pointCollection.Aggregate((min, cur) => cur.Y < min.Y ? cur : min);
            Point3d minZPoint = pointCollection.Aggregate((min, cur) => cur.Z < min.Z ? cur : min);

            return new Point3d(minXPoint.X, minYPoint.Y, minZPoint.Z);
        }
        private Point3d CalculateMaxPoint()
        {
            Point3d maxXPoint = pointCollection.Aggregate((max, cur) => cur.X > max.X ? cur : max);
            Point3d maxYPoint = pointCollection.Aggregate((max, cur) => cur.Y > max.Y ? cur : max);
            Point3d maxZPoint = pointCollection.Aggregate((max, cur) => cur.Z > max.Z ? cur : max);

            return new Point3d(maxXPoint.X, maxYPoint.Y, maxZPoint.Z);
        }

        public bool IsOutOfBoundary(Point3d point)
        {
            if (point.X < MinPoint.X)
            {
                return true;
            }
            else if (point.X > MaxPoint.X)
            {
                return true;
            }
            else if (point.Y < MinPoint.Y)
            {
                return true;
            }
            else if (point.Y > MaxPoint.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }     

        private void CreateExtrusion()
        {
            Part workPart = Session.GetSession().Parts.Work;
            BoundedPlaneBuilder boundedPlane = workPart.Features.CreateBoundedPlaneBuilder(null);
            SelectionIntentRule[] rule1 = new SelectionIntentRule[1];            

            boundedPlane.CommitFeature();
        }
    }
}
