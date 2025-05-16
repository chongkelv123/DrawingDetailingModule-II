using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace DrawingDetailingModule.Interfaces
{
    public interface ISelectionService
    {
        // Properties
        bool IsFaceSelected { get; }
        bool IsPointLocated { get; }
        List<TaggedObject> SelectedBody { get; set; }
        List<Point3d> LocatedPoint { get; set; }

        // Operations
        List<TaggedObject> SelectBody();
        List<Point3d> SelectScreenPosition();
        void ClearSelections();
    }
}
