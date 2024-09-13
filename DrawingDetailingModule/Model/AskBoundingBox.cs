using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.UF;
using NXOpen;
using NXOpen.Features;
using NXOpen.Utilities;

namespace DrawingDetailingModule.Model
{
    public enum AXIS
    {
        X = 1, Y = 2, Z = 3,
    }
    public class AskBoundingBox
    {
        double[] boundingBox = new double[6];
        double precision = -0.0001;

        UFSession ufs;

        public double MinX
        {
            get { return boundingBox[0]; }
            set { boundingBox[0] = value; }
        }
        public double MinY
        {
            get { return boundingBox[1]; }
            set { boundingBox[1] = value; }
        }
        public double MinZ
        {
            get { return boundingBox[2]; }
            set { boundingBox[2] = value; }
        }
        public double MaxX
        {
            get { return boundingBox[3]; }
            set { boundingBox[3] = value; }
        }
        public double MaxY
        {
            get { return boundingBox[4]; }
            set { boundingBox[4] = value; }
        }
        public double MaxZ
        {
            get { return boundingBox[5]; }
            set { boundingBox[5] = value; }
        }
        public AskBoundingBox(UFSession ufs, Tag boundingBoxTag)
        {
            this.ufs = ufs;
            this.ufs.Modl.AskBoundingBox(boundingBoxTag, boundingBox);
        }

        public AskBoundingBox()
        {
            UFSession ufs = UFSession.GetUFSession();
            this.ufs = ufs;
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

        public NXObject CreateBoundingBox()
        {
            FeatureSigns sign = FeatureSigns.Nullsign;
            double[] corner_pt = new double[] { MinX, MinY, MinZ };

            string thickness = GetThickness();
            string length = GetLength();
            string width = GetWidth();

            string[] edge_len = new string[] { length, width, thickness };
            Tag blk_obj;
            ufs.Modl.CreateBlock1(sign, corner_pt, edge_len, out blk_obj);

            NXObject result = NXOpen.Utilities.NXObjectManager.Get(blk_obj) as NXObject;
            return result;
        }

        public string GetLength()
        {
            if (IsEqualZero(precision, MaxX))
            {
                MaxX = 0;
            }
            if (MinX < 0)
            {
                return (MaxX - MinX).ToString();
            }
            if (MaxX > 0)
            {
                return Math.Abs(MaxX).ToString();
            }
            return Math.Abs(MinX).ToString();
        }

        public string GetWidth()
        {
            if (IsEqualZero(precision, MaxY))
            {
                MaxY = 0;
            }
            if (MinY < 0)
            {
                return (MaxY - MinY).ToString();
            }
            if (MaxY > 0)
            {
                return Math.Abs(MaxY).ToString();
            }
            return Math.Abs(MinY).ToString();
        }

        public string GetThickness()
        {
            if (IsEqualZero(precision, MaxZ))
            {
                MaxZ = 0;
            }
            if (MinZ < 0)
            {
                return (MaxZ - MinZ).ToString();
            }
            if (MaxZ > 0)
            {
                return Math.Abs(MaxZ).ToString();
            }
            return Math.Abs(MinZ).ToString();
        }

        private bool IsEqualZero(double precision, double eval)
        {
            return Math.Abs(eval) > precision &&
                Math.Abs(eval) < Math.Abs(precision);
        }

        public double[] AskDirection(double[] point, AXIS Axis)
        {
            double[] result = new double[3];
            result[0] = 0;
            result[1] = 0;
            result[2] = 0;

            switch (Axis)
            {
                case AXIS.X:
                    break;
                case AXIS.Y:
                    break;
                case AXIS.Z:
                    if (MinZ == point[2])
                    {
                        result[2] = 1;
                    }
                    else if (MaxZ == point[2])
                    {
                        result[2] = -1;
                    }
                    else if (MinZ < point[2])
                    {
                        result[2] = -1;
                    }
                    else if (MaxZ > point[2])
                    {
                        result[2] = 1;
                    }
                    break;
                default:
                    throw new ArgumentException("Wrong Axis supplied in AskBoudingBox");
            }

            return result;
        }

        public bool IsPointContainInBoundary (Point3d p, Tag SelectedBodyTag)
        {
            const int INSIDE_BODY = 1;
            const int OUTSIDE_BODY = 2;
            const int ON_BODY = 3;

            int pt_status = 0;
            AskBoundingBox boundingBox = new AskBoundingBox(ufs, SelectedBodyTag);
            TaggedObject obj = NXObjectManager.Get(SelectedBodyTag);
            Body body = obj as Body;
            Body[] bodies = { body };

            double[] pt = new double[] { p.X, p.Y, p.Z };

            ufs.Modl.AskPointContainment(pt, bodies[0].Tag, out pt_status);

            if (pt_status == OUTSIDE_BODY)
            {
                return false;
            }
            else if (pt_status == ON_BODY)
            {
                return true;
            }
            else if (pt_status == INSIDE_BODY)
            {
                return true;
            }

            return false;
        }
    }
}
