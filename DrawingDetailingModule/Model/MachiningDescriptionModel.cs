using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NXOpen;

namespace DrawingDetailingModule.Model
{
    public class MachiningDescriptionModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Abbrevate { get; set; }
        public double[] Direction { get; set; }
        public List<Point3d> Points { get; set; }
        public string Height { get; set; }

        public MachiningDescriptionModel()
        {
        }

        public MachiningDescriptionModel(string description, int quantity, List<Point3d> points, string abbrevate, double[] direction, string height)
        {
            Description = description;
            Quantity = quantity;
            Points = points;
            Abbrevate = abbrevate;
            Direction = direction;
            Height = height;
        }

        public static bool IsDescriptionSame(List<MachiningDescriptionModel> models, MachiningDescriptionModel targetModel)
        {
            foreach (MachiningDescriptionModel model in models)
            {
                if (model.Description.Equals(targetModel.Description, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static void SumUpModelQuantity(List<MachiningDescriptionModel> models, MachiningDescriptionModel targetModel)
        {
            foreach (MachiningDescriptionModel model in models)
            {
                if (model.Description.Equals(targetModel.Description, StringComparison.OrdinalIgnoreCase))
                {
                    model.Quantity = targetModel.Quantity + model.Quantity;
                }
            }
        }

        public static void AppendModelPoints(List<MachiningDescriptionModel> models, MachiningDescriptionModel targetModel)
        {
            foreach (MachiningDescriptionModel model in models)
            {
                if (model.Description.Equals(targetModel.Description, StringComparison.OrdinalIgnoreCase))
                {
                    targetModel.Points.ForEach(x => model.Points.Add(x));
                }
            }
        }

        public double GetWCStartPointDiameter(string input)
        {
            string pattern = @"\(<[oO]>(\d+(\.\d+)?) wc sp\)";
            string numberStr = "";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                numberStr = match.Groups[1].Value;
            }

            double result;
            Double.TryParse(numberStr, out result);

            return result;
        }

        public double GetWCHoleDiameter(string input)
        {
            string pattern = @"WC <[Oo]>(\d+(\.\d+)?)";
            string numberStr = "";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                numberStr = match.Groups[1].Value;
            }

            double result;
            Double.TryParse(numberStr, out result);

            return result;
        }
    }
}
