using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Service for handling UI operations and user interactions
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Shows a message box to the user
        /// </summary>
        /// <param name="title">The title of the message box</param>
        /// <param name="msgboxType">The type of message box</param>
        /// <param name="message">The message to display</param>
        void ShowMessageBox(string title, NXMessageBox.DialogType msgboxType, string message);

        /// <summary>
        /// Checks if a drawing is currently open
        /// </summary>
        /// <returns>True if a drawing is open, otherwise false</returns>
        bool IsDrawingOpen();

        /// <summary>
        /// Checks if the current part is loaded
        /// </summary>
        /// <returns>True if the part is loaded and can be accessed</returns>
        bool IsPartLoaded();

        /// <summary>
        /// Shows an information message to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        void ShowInfo(string message);

        /// <summary>
        /// Shows an error message to the user
        /// </summary>
        /// <param name="message">The error message to display</param>
        void ShowError(string message);

        /// <summary>
        /// Shows a warning message to the user
        /// </summary>
        /// <param name="message">The warning message to display</param>
        void ShowWarning(string message);
    }
}
