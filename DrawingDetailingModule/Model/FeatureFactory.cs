using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.CAM;
using NXOpen.Features;
using DrawingDetailingModule.Interfaces;

namespace DrawingDetailingModule.Model
{
    public class FeatureFactory:IAbstractFeatureFactory
    {
        public const string THREADED = "THREADED";
        public const string DRILL = "DRILL";
        public const string COUNTERBORED = "COUNTERBORED";
        public const string COUNTERSUNK = "COUNTERSUNK";
        public const string SIMPLE = "SIMPLE";
        public const string MACHINING = "Machining";
        public const string TYPE = "Type";
        public const string REAM = "REAM";
        public const string WC = "WC";
        public const string MILL = "MILL";
        public const string MIRROR = "MIRROR";
        public const string WC_OFFSET = "wcOffset";
        public const string CUT_CONDITION = "cutCondition";
        public const string EXTRUDE = "EXTRUDE";
        public const string DR = "DR";
        public const string CBORE = "C'BORE";
        public const string WC_SP = "wc sp";
        public const string DP = "DP";
        public const string SYMBOLIC_THREAD = "SYMBOLIC_THREAD";
        public const string PATTERN = "Pattern";
        public const string SLANT_CUT = "SlantCut";

        private readonly Part workPart;
        public FeatureFactory()
        {
            workPart = Session.GetSession().Parts.Work;
        }

        public MyFeature CreateFeature(Feature feature)
        {            
            string processType = MyFeature.GetProcessType(feature);
            HolePackage holePackage = feature as HolePackage;
            
            AttributeIterator iterator = workPart.CreateAttributeIterator();
            iterator.SetIncludeOnlyTitle(TYPE);
            
            switch (processType)
            {
                case THREADED:
                    return CreateThreadedHole(holePackage);
                case COUNTERBORED:
                    return CreateCounterboreHole(feature, GetFeatureType(feature, iterator));
                case SIMPLE:
                    return CreateSimpleHole(holePackage, GetFeatureType(feature, iterator));
                case DRILL:
                    return new SimpleHole2(holePackage);
                case EXTRUDE:
                    return CreatePocketFeature(feature, GetFeatureType(feature, iterator));
                case COUNTERSUNK:
                    return CreateCountersunkHole(feature);
                default:
                    throw new ArgumentNullException($"Unsupported feature type: {processType}", nameof(processType));
            }

        }

        // IHoleFeatureFactory implementation
        public MyHoleFeature CreateSimpleHole(HolePackage holePackage, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return new SimpleHole2(holePackage);
            }

            if (type.Equals(REAM, StringComparison.OrdinalIgnoreCase))
            {
                return new ReamSimpleHole(holePackage);
            }

            if (type.Equals(MILL, StringComparison.OrdinalIgnoreCase))
            {
                return new MillSimpleHole(holePackage);
            }

            return new WCSimpleHole(holePackage);
        }

        public MyHoleFeature CreateCounterboreHole(Feature feature, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return new Counterbore2(feature);
            }

            if (type.Equals(REAM, StringComparison.OrdinalIgnoreCase))
            {
                return new ReamCounterbore(feature);
            }

            return new WCCounterbore(feature);
        }

        public CounterSunk CreateCountersunkHole(Feature feature)
        {
            return new CounterSunk(feature);
        }

        public Threaded2 CreateThreadedHole(HolePackage holePackage)
        {
            return new Threaded2(holePackage);
        }

        // IPocketFeatureFactory implementation
        public MyPocketFeature CreatePocketFeature(Feature feature, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type), "Type cannot be null for pocket features");
            }

            if (type.Equals(WC, StringComparison.OrdinalIgnoreCase))
            {
                return new WCPocketFeature(feature);
            }

            if (type.Equals(MILL, StringComparison.OrdinalIgnoreCase))
            {
                return new MillPocketFeature(feature);
            }

            throw new ArgumentException($"Unsupported pocket feature type: {type}", nameof(type));
        }

        // Helper method to get feature type from attributes
        private string GetFeatureType(Feature feature, AttributeIterator iterator)
        {
            if (feature.HasUserAttribute(iterator))
            {
                return feature.GetStringUserAttribute(TYPE, 0);
            }
            return string.Empty;
        }
    }
}
