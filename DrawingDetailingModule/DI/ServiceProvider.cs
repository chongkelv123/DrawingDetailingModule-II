using DrawingDetailingModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.DI
{
    /// <summary>
    /// Provides easy access to commonly used services
    /// </summary>
    public class ServiceProvider
    {
        /// <summary>
        /// Gets the selection service
        /// </summary>
        public static ISelectionService SelectionService => ServiceLocator.Resolve<ISelectionService>();

        /// <summary>
        /// Gets the NX session provider
        /// </summary>
        public static INXSessionProvider SessionProvider => ServiceLocator.Resolve<INXSessionProvider>();

        /// <summary>
        /// Gets the feature processor
        /// </summary>
        public static IFeatureProcessor FeatureProcessor => ServiceLocator.Resolve<IFeatureProcessor>();

        /// <summary>
        /// Gets the table service
        /// </summary>
        public static ITableService TableService => ServiceLocator.Resolve<ITableService>();

        /// <summary>
        /// Gets the UI service
        /// </summary>
        public static IUIService UIService => ServiceLocator.Resolve<IUIService>();

        /// <summary>
        /// Gets the feature factory
        /// </summary>
        public static IFeatureFactory FeatureFactory => ServiceLocator.Resolve<IFeatureFactory>();

        /// <summary>
        /// Gets the hole feature factory
        /// </summary>
        public static IHoleFeatureFactory HoleFeatureFactory => ServiceLocator.Resolve<IHoleFeatureFactory>();

        /// <summary>
        /// Gets the pocket feature factory
        /// </summary>
        public static IPocketFeatureFactory PocketFeatureFactory => ServiceLocator.Resolve<IPocketFeatureFactory>();

        /// <summary>
        /// Gets the abstract feature factory
        /// </summary>
        public static IAbstractFeatureFactory AbstractFeatureFactory => ServiceLocator.Resolve<IAbstractFeatureFactory>();
    }
}
