using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;
using DrawingDetailingModule.View;

namespace DrawingDetailingModule.Controller
{
    public class Control
    {
        NXDrawing drawing;
        FormDrawingDetailing myForm;

        public NXDrawing GetDrawing => drawing;
        public FormDrawingDetailing GetForm => myForm;

        public Control()
        {
            drawing = new NXDrawing(this);
            if (!drawing.IsDrawingOpen())
            {
                return;
            }
            myForm = new FormDrawingDetailing(this);
            myForm.Show();
        }

        public void Start()
        {
            double currentTextSize = GetDrawing.GetCurrentTextSize();
            GetDrawing.SetTextSize(myForm.FontSize);
            try
            {
                GetDrawing.IterateFeatures();
                GetDrawing.CreateTable(GetDrawing.LocatedPoint[0]);
            }
            catch (Exception err)
            {
                GetDrawing.ShowMessageBox("Error", NXOpen.NXMessageBox.DialogType.Error, $"Error detail: {err.Message}");
            }
            GetDrawing.SetTextSize(currentTextSize);
        }
    }
}
