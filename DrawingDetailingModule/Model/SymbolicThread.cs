using NXOpen.Features;
using NXOpen.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static NXOpen.Weld.WeldGrooveBuilder;

namespace DrawingDetailingModule.Model
{
    public class SymbolicThread
    {
        public double Length { get; set; }
        public double Pitch { get; set; }
        public double Angle { get; set; }
        public double MajorDiameter { get; set; }
        public double MinorDiameter { get; set; }
        public double TapDrillSize { get; set; }

        const string LENGTH = "Length";
        const string PITCH = "Pitch";
        const string ANGLE = "Angle";
        const string MAJOR_DIAMETER = "Major Diameter";
        const string MINOR_DIAMETER = "Minor Diameter";
        const string TAPPED_DRILL_SIZE = "Tapped Drill Size";

        Feature feature;

        public SymbolicThread() { }

        public bool IsSymbolicThreads(Feature feature)
        {
            this.feature = feature;
            string processType = MyFeature.GetProcessType(feature);
            if (!processType.Equals(FeatureFactory.SYMBOLIC_THREAD))
            {
                return false;
            }
            processThreadInfo();
            return true;
        }

        private void processThreadInfo()
        {            
            var exprs = feature.GetExpressions();
            if (exprs == null)
            {
                return;
            }
            Dictionary<string, double> keyValues =  processExprToDict(exprs);

            Length = keyValues[LENGTH];
            Pitch = keyValues[PITCH];
            Angle = keyValues[ANGLE];
            MajorDiameter = keyValues[MAJOR_DIAMETER];
            MinorDiameter = keyValues[MINOR_DIAMETER];
            TapDrillSize = keyValues[TAPPED_DRILL_SIZE];
        }

        private Dictionary<string, double> processExprToDict(NXOpen.Expression[] exprs)
        {
            string pattern = @"\(.*\(.*\) (.+?)\)";
            Dictionary<string, double> keyValues = new Dictionary<string, double>();
            foreach (var expr in exprs)
            {
                Match match = Regex.Match(expr.Description, pattern);
                if (match.Success)
                {
                    var key = match.Groups[1].Value;
                    keyValues.Add(key, expr.Value);                    
                }
            }

            return keyValues;
        }

        public bool IsContains(List<SymbolicThread> threads, SymbolicThread thread)
        {
            if (threads.Count == 0)
            {
                return false;
            }

            foreach (SymbolicThread item in threads)
            {
                if(item.MajorDiameter == thread.MajorDiameter && 
                    item.Length == thread.Length && 
                    item.Pitch == thread.Pitch)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
