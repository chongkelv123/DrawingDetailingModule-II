using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;
using NXOpen;
using NXOpen.Features;

namespace DrawingDetailingModule.Interfaces
{
    public interface IFeatureProcessor
    {
        /// <summary>
        /// Iterates through all features in the current work part and processes them
        /// </summary>
        /// <returns>List of machining description models</returns>
        List<MachiningDescriptionModel> IterateFeatures();

        /// <summary>
        /// Processes a hole package feature
        /// </summary>
        /// <param name="factory">The feature factory</param>
        /// <param name="feature">The hole package feature</param>
        /// <returns>A machining description model</returns>
        MachiningDescriptionModel ProcessHolePackage(IFeatureFactory factory, Feature feature);

        /// <summary>
        /// Processes a wire cut feature
        /// </summary>
        /// <param name="factory">The feature factory</param>
        /// <param name="feature">The extrude feature</param>
        /// <returns>A machining description model</returns>
        MachiningDescriptionModel ProcessWCFeat(IFeatureFactory factory, Feature feature);

        /// <summary>
        /// Processes a mill feature
        /// </summary>
        /// <param name="factory">The feature factory</param>
        /// <param name="feature">The extrude feature</param>
        /// <returns>A machining description model</returns>
        MachiningDescriptionModel ProcessMillFeat(IFeatureFactory factory, Feature feature);

        /// <summary>
        /// Generates wire cut start points for the provided description models
        /// </summary>
        /// <param name="descriptionModels">The machining description models</param>
        void GenerateWCStartPoints(List<MachiningDescriptionModel> descriptionModels);
    }
}
