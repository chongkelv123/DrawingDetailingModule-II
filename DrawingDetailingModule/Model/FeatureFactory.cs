using System;
using System.CodeDom;
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

            switch (processType)
            {
                case THREADED:
                    return new Threaded2(holePackage);
                case COUNTERBORED:
                    return CounterBoreClassification(feature, iterator);
                case SIMPLE:
                    return SimpleHoleClassification(feature, iterator);
                case DRILL:
                    return new SimpleHole2(holePackage);
                case EXTRUDE:
                    return PocketFeatureClassification(feature, iterator);
                case COUNTERSUNK:
                    return new CounterSunk(feature);
                default:
                    throw new ArgumentNullException($"Error on: {processType}");
            }

        }

        private MyFeature PocketFeatureClassification(Feature feature, AttributeIterator iterator)
        {
            string type;
            if (!feature.HasUserAttribute(iterator))
            {
                throw new ArgumentNullException($"Error on: PocketFeatureClassifiction, no Type Value in Attribute.");
            }

            type = feature.GetStringUserAttribute(TYPE, 0);

            if (type.Equals(FeatureFactory.WC, StringComparison.OrdinalIgnoreCase))
            {
                return new WCPocketFeature(feature);
            }
            else if (type.Equals(FeatureFactory.MILL, StringComparison.OrdinalIgnoreCase))
            {
                return new MillPocketFeature(feature);
            }
            throw new ArgumentNullException($"Error on: PocketFeatureClassifiction, Type Value is arbitrary.");
        }

        private MyFeature SimpleHoleClassification(Feature feature, AttributeIterator iterator)
        {
            HolePackage holePackage = feature as HolePackage;
            string type;

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
