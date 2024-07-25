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
        }

        protected MyHoleFeature()
        {
        }
    }
}
