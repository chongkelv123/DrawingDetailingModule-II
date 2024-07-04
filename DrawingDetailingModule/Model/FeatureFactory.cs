using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
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
        public const string WC_OFFSET = "wcOffset";
        public const string WC_CONDITION = "wcCondition";
        public const string EXTRUDE = "EXTRUDE";

        public FeatureFactory()
        {
        }

        public MyFeature GetFeature(Feature feature)
        {
            string processType = MyFeature.GetProcessType(feature);
            HolePackage holePackage = feature as HolePackage;

            Part part = Session.GetSession().Parts.Work;
            AttributeIterator iterator = part.CreateAttributeIterator();
            iterator.SetIncludeOnlyCategory(MACHINING);

            switch (processType)
            {
                case THREADED:
                    return new Threaded2(holePackage);
                case COUNTERBORED:
                    return new Counterbore2(holePackage);
                case SIMPLE:
                    if (feature.HasUserAttribute(iterator))
                    {
                        string type = feature.GetStringUserAttribute(TYPE, 0);
                        if (type.Equals(REAM, StringComparison.OrdinalIgnoreCase))
                        {
                            return new ReamSimpleHole(holePackage);
                        }
                        else
                        {
                            return new WCSimpleHole(holePackage);
                        }
                    }                   
                    return new SimpleHole2(holePackage);
                default:
                    throw new Exception("Feature Factory Exception: did not catch error");
            }

        }
    }
}
