using DrawingDetailingModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.Features;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Interface for creating feature objects
    /// </summary>
    public interface IFeatureFactory
    {
        /// <summary>
        /// Creates an appropriate feature object based on the NX feature
        /// </summary>
        /// <param name="feature">The NX feature to process</param>
        /// <returns>A MyFeature object</returns>
        MyFeature CreateFeature(Feature feature);
    }

    /// <summary>
    /// Interface for creating hole feature objects
    /// </summary>
    public interface IHoleFeatureFactory
    {
        /// <summary>
        /// Creates a simple hole feature
        /// </summary>
        /// <param name="holePackage">The hole package</param>
        /// <param name="type">The type of hole</param>
        /// <returns>A MyHoleFeature object</returns>
        MyHoleFeature CreateSimpleHole(HolePackage holePackage, string type);

        /// <summary>
        /// Creates a counterbore hole feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <param name="type">The type of counterbore</param>
        /// <returns>A MyHoleFeature object</returns>
        MyHoleFeature CreateCounterboreHole(Feature feature, string type);

        /// <summary>
        /// Creates a countersunk hole feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A CounterSunk object</returns>
        CounterSunk CreateCountersunkHole(Feature feature);

        /// <summary>
        /// Creates a threaded hole feature
        /// </summary>
        /// <param name="holePackage">The hole package</param>
        /// <returns>A Threaded2 object</returns>
        Threaded2 CreateThreadedHole(HolePackage holePackage);
    }

    /// <summary>
    /// Interface for creating pocket feature objects
    /// </summary>
    public interface IPocketFeatureFactory
    {
        /// <summary>
        /// Creates a pocket feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <param name="type">The type of pocket</param>
        /// <returns>A MyPocketFeature object</returns>
        MyPocketFeature CreatePocketFeature(Feature feature, string type);
    }
}
