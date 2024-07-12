using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace DrawingDetailingModule.Model
{
    public class MachiningDescriptionModel
    {
        public string Description {get; set;}
        public int  Quantity { get; set; }
        public List<Point3d> Points { get; set; }
        
        public MachiningDescriptionModel()
        {
        }

        public MachiningDescriptionModel(string description, int quantity, List<Point3d> points)
        {
            Description = description;
            Quantity = quantity;
            Points = points;
        }
    }
}
