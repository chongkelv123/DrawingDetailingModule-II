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
    public abstract class MyHoleFeature : MyFeature
    {
        public double HoleDiameter { get; set; }
        public double HoleDepth { get; set; }
        public double TipAngle { get; set; }
        public bool IsThruHole { get; set; }

        protected MyHoleFeature(Feature feature) : base(feature)
        {
            GetCenterPoints(feature);
        }

        protected MyHoleFeature()
        {
        }
        public void GetCenterPoints(Feature feature)
        {
            HolePackage holePackage = feature as HolePackage;
            Point3d[] origins;
            holePackage.GetOrigins(out origins);
            origins.ToList().ForEach(x => points.Add(new Point3d(x.X, x.Y, x.Z)));
        }

        public List<Point3d> GetLocation()
        {
            return points;
        }
        public List<SymbolicThread> GetSymbolicThread(Feature[] allChilds, List<SymbolicThread> SymbolicThreads)
        {            
            List<SymbolicThread> result = new List<SymbolicThread>();
            if (allChilds.Length > 0)
            {
                foreach (Feature child in allChilds)
                {
                    SymbolicThread symbolicThread = new SymbolicThread();
                    if (!symbolicThread.IsSymbolicThreads(child))
                    {
                        continue;
                    }
                    if (!symbolicThread.IsContains(result, symbolicThread))
                    {
                        result.Add(symbolicThread);
                    }
                }
            }
            return result;
        }


    }
}
