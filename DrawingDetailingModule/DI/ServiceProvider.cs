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
    }
}
