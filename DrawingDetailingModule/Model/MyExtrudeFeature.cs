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
    public abstract class MyExtrudeFeature : MyFeature
    {
        protected MyExtrudeFeature(Feature feature) : base(feature)
        {
        }

        protected MyExtrudeFeature()
        {
        }
    }
}
