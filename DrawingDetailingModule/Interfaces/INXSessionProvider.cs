using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.UF;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Provides access to NX sessions and centralizes NX API interactions
    /// </summary>
    public interface INXSessionProvider
    {
        /// <summary>
        /// Gets the current NX session
        /// </summary>
        Session GetSession();

        /// <summary>
        /// Gets the UF session
        /// </summary>
        UFSession GetUFSession();

        /// <summary>
        /// Gets the current work part
        /// </summary>
        Part GetWorkPart();

        /// <summary>
        /// Gets the UI interface
        /// </summary>
        UI GetUI();

        /// <summary>
        /// Sets the text size in the current work part
        /// </summary>
        /// <param name="size">The size to set</param>
        void SetTextSize(double size);

        /// <summary>
        /// Gets the current text size
        /// </summary>
        /// <returns>The current text size</returns>
        double GetCurrentTextSize();

        /// <summary>
        /// Gets the dimension text size from preferences
        /// </summary>
        /// <returns>The dimension text size</returns>
        int GetDimensionTextSize();

        /// <summary>
        /// Shows a message box using NX UI
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
    }
}
