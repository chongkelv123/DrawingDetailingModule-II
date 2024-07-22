using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.UF;
using NXOpen;
using NXOpen.Utilities;

namespace DrawingDetailingModule.Model
{
    public class AskBoundingBox
    {
        double[] boundingBox = new double[6];

        UFSession ufs;

        public double MinX
        {
            get { return boundingBox[0]; }
        }
        public double MinY
        {
            get { return boundingBox[1]; }
        }
        public double MinZ
        {
            get { return boundingBox[2]; }
        }
        public double MaxX
        {
            get { return boundingBox[3]; }
        }
        public double MaxY
        {
            get { return boundingBox[4]; }
        }
        public double MaxZ
        {
            get { return boundingBox[5]; }
        }
        public AskBoundingBox(UFSession ufs, Tag boundingBoxTag)
        {
            this.ufs = ufs;
            this.ufs.Modl.AskBoundingBox(boundingBoxTag, boundingBox);
        }

        public bool IsFromTopDirection(double ptZ)
        {
            return (MaxZ == ptZ);
        }
        public bool IsFromBottomDirection(double ptZ)
        {
            return (MinZ == ptZ);
        }
        public bool IsFromLeftDirection(double ptX)
        {
            return (MinX == ptX);
        }
        public bool IsFromRightDirection(double ptX)
        {
            return (MaxX == ptX);
        }
        public bool IsFromFrontDirection(double ptY)
        {
            return (MinY == ptY);
        }
        public bool IsFromBackDirection(double ptY)
        {
            return (MaxY == ptY);
        }

        public List<Point3d> VerifyPoints(List<Point3d> pointCollection)
        {
            List<Point3d> outPoints = new List<Point3d>();
            HashSet<Point2d> tempPoints = new HashSet<Point2d>();
            double zValue = -1;
            foreach (Point3d p in pointCollection)
            {
                tempPoints.Add(new Point2d(p.X, p.Y));
                if (zValue == -1)
                {
                    zValue = p.Z;
                }
            }
            tempPoints.ToList().ForEach(x => outPoints.Add(new Point3d(x.X, x.Y, zValue)));
            return outPoints;
        }

        public NXObject CreateBoundingBox()
        {
            //System.Diagnostics.Debugger.Launch();
            FeatureSigns sign = FeatureSigns.Nullsign;
            double[] corner_pt = new double[] { MinX, MinY, MinZ };

            string thickness = GetThickness();
            string length = GetLength();
            string width = GetWidth();

            string[] edge_len = new string[] { thickness, length, width };
            Tag blk_obj;
            ufs.Modl.CreateBlock1(sign, corner_pt, edge_len, out blk_obj);

            NXObject result = NXOpen.Utilities.NXObjectManager.Get(blk_obj) as NXObject;
            return result;
        }

        private string GetWidth()
        {
            if (MaxY > 0)
            {
                return Math.Abs(MaxY).ToString();
            }
            return Math.Abs(MinY).ToString();
        }

        private string GetLength()
        {
            if(MaxX > 0)
            {
                return Math.Abs(MaxX).ToString();
            }
            return Math.Abs(MinX).ToString();
        }

        private string GetThickness()
        {
            if(MaxZ > 0)
            {
                return Math.Abs(MaxZ).ToString();
            }
            return Math.Abs(MinZ).ToString();
        }
    }
}
