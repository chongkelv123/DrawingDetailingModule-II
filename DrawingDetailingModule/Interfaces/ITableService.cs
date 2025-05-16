using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;
using NXOpen;
using NXOpen.Annotations;
using NXOpen.Features;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Service for creating and managing tables and annotations
    /// </summary>
    public interface ITableService
    {
        /// <summary>
        /// Creates a table with machining descriptions
        /// </summary>
        /// <param name="insertionPoint">The point where the table will be inserted</param>
        /// <param name="descriptionModels">The list of machining description models</param>
        /// <returns>The created table section</returns>
        TableSection CreateTable(Point3d insertionPoint, List<MachiningDescriptionModel> descriptionModels);

        /// <summary>
        /// Places annotation at specified points
        /// </summary>
        /// <param name="points">The points where annotations will be placed</param>
        /// <param name="alphabet">The text to display in the annotation</param>
        void PlaceAnnotation(List<Point3d> points, string alphabet);

        /// <summary>
        /// Converts a number to an alphabetic identifier
        /// </summary>
        /// <param name="number">The number to convert</param>
        /// <returns>The alphabetic representation</returns>
        string NumberToAlphabet(int number);

        /// <summary>
        /// Creates a wire cut start point cylinder
        /// </summary>
        /// <param name="workPart">The current work part</param>
        /// <param name="basePoint">The base point for the cylinder</param>
        /// <param name="height">The height of the cylinder</param>
        /// <param name="diameter">The diameter of the cylinder</param>
        /// <param name="direction">The direction vector for the cylinder</param>
        /// <returns>The created cylinder</returns>
        Cylinder CreateWCStartPoint(Part workPart, Point3d basePoint, string height, double diameter, double[] direction);
    }
}
