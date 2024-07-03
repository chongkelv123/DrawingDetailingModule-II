using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class FeatureFactory
    {        

        public const string THREADED = "THREADED";
        public const string COUNTERBORED = "COUNTERBORED";
        public const string SIMPLE = "SIMPLE";
        public const string MACHINING = "Machining";
        public const string TYPE = "Type";
        public const string REAM = "REAM";
        public const string WC = "WC";

        public FeatureFactory()
        {            
        }

        public MyFeature GetFeature(HolePackage holePackage)
        {
            string processType = MyFeature.GetProcessType(holePackage);
            switch (processType)
            {
                case THREADED:
                    return new Threaded2(holePackage);
                case COUNTERBORED:
                    return new Counterbore2(holePackage);
                case SIMPLE:
                    return new SimpleHole2(holePackage);
                default:
                    throw new Exception("Feature Factory Exception: did not catch error");
            }

        }
    }
}
