using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.UF;
using NXOpenUI;

using DrawingDetailingModule.Controller;

namespace DrawingDetailingModule.Model
{
    public class NXDrawing
    {
        Session session;
        Part workPart;
        UI ui;
        Control control;
        public NXDrawing(Control control)
        {
            session = Session.GetSession();
            workPart = session.Parts.Work;
            ui = UI.GetUI();

            this.control = control;
        }

        public bool IsDrawingOpen()
        {
            //System.Diagnostics.Debugger.Launch();

            string title = "No active drawing";
            string message = "You accidentally launched the BOM command by mistake and ";
            NXMessageBox.DialogType msgboxType = NXMessageBox.DialogType.Warning;

            Part displayPart = session.Parts.Display;
            bool isNoCompnent = displayPart == null;

            if (isNoCompnent)
            {
                message += " you are not open any drawings yet! ;-)";
                ShowMessageBox(title, msgboxType, message);
                return false;
            }

            return true;
        }

        public void ShowMessageBox(string title, NXMessageBox.DialogType msgboxType, string message)
        {
            ui.NXMessageBox.Show(title, msgboxType, message);
        }
    }
}
