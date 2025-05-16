using System;

using DrawingDetailingModule.Controller;
using DrawingDetailingModule.DI;
using DrawingDetailingModule.Interfaces;
using DrawingDetailingModule.Model;

namespace DrawingDetailingModule
{
    public partial class MyProgram
    {
        private static void RegisterServices()
        {
            // Create the core service implementation
            var drawing = new NXDrawing();

            // Register interfaces
            ServiceLocator.Register<ISelectionService>(drawing);
            ServiceLocator.Register<INXSessionProvider>(drawing);
            ServiceLocator.Register<IFeatureProcessor>(drawing);
            ServiceLocator.Register<ITableService>(drawing);
            ServiceLocator.Register<IUIService>(drawing);

            // Register the feature factory
            ServiceLocator.Register<IAbstractFeatureFactory>(new FeatureFactory());
            // Also register the individual interfaces
            ServiceLocator.Register<IFeatureFactory>(ServiceLocator.Resolve<IAbstractFeatureFactory>());
            ServiceLocator.Register<IHoleFeatureFactory>(ServiceLocator.Resolve<IAbstractFeatureFactory>());
            ServiceLocator.Register<IPocketFeatureFactory>(ServiceLocator.Resolve<IAbstractFeatureFactory>());

            // Register the controller - we'll update this class in the next step
            ServiceLocator.RegisterFactory<IController>(() => new Control());
        }

        public static void Main(string[] args)
        {
            // Register all services
            RegisterServices();

            // Get the controller through the DI container
            var control = ServiceLocator.Resolve<IController>();

            // Start the application
            control.Initialize();
        }
    }
}
