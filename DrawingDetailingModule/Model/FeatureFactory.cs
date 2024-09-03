using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.CAM;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class FeatureFactory
    {
        public const string THREADED = "THREADED";
        public const string DRILL = "DRILL";
        public const string COUNTERBORED = "COUNTERBORED";
        public const string SIMPLE = "SIMPLE";
        public const string MACHINING = "Machining";
        public const string TYPE = "Type";
        public const string REAM = "REAM";
        public const string WC = "WC";
        public const string WC_OFFSET = "wcOffset";
        public const string WC_CONDITION = "wcCondition";
        public const string EXTRUDE = "EXTRUDE";
        public const string DR = "DR";
        public const string CBORE = "C'BORE";
        public const string WC_SP = "wc sp";
        public const string DP = "DP";

        public FeatureFactory()
        {
        }

        public MyFeature GetFeature(Feature feature)
        {
            string processType = MyFeature.GetProcessType(feature);
            HolePackage holePackage = feature as HolePackage;

            Part part = Session.GetSession().Parts.Work;
            AttributeIterator iterator = part.CreateAttributeIterator();
            iterator.SetIncludeOnlyTitle(TYPE);
            string type;
            
            switch (processType)
            {
                case THREADED:
                    return new Threaded2(holePackage);
                case COUNTERBORED:                    
                    return CounterBoreClassification(feature, iterator);
                case SIMPLE:
                    if (!feature.HasUserAttribute(iterator))
                    {
                        return new SimpleHole2(holePackage);
                    }

                    type = feature.GetStringUserAttribute(TYPE, 0);
                    if (type.Equals(REAM, StringComparison.OrdinalIgnoreCase))
                    {
                        return new ReamSimpleHole(holePackage);
                    }

                    return new WCSimpleHole(holePackage);
                case DRILL:
                    return new SimpleHole2(holePackage);                    
                default:
                    return null;
            }

        }

        private MyFeature CounterBoreClassification(Feature feature, AttributeIterator iterator)
        {            
            if (!feature.HasUserAttribute(iterator))
            {
                return new Counterbore2(feature);
            }

            string type = feature.GetStringUserAttribute(TYPE, 0);
            if (type.Equals(REAM, StringComparison.OrdinalIgnoreCase))
            {
                return new ReamCounterbore(feature);
            }

            return new WCCounterbore(feature);
        }
    }
}
