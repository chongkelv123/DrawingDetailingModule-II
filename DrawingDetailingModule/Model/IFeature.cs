using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    interface IFeature
    {
        string GetProcessType(HolePackage holePackage);
        string GetProcessAbbrevate();
        void GetFeatureDetailInformation(HolePackage holePackage);

    }
}
