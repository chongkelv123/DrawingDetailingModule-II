using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Interface for the main application controller
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Initializes the controller and shows the main form
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts the drawing detailing process
        /// </summary>
        void Start();

        /// <summary>
        /// Gets the dimension text size
        /// </summary>
        /// <returns>The dimension text size</returns>
        int GetDimensionTextSize();
    }
}
