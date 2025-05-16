using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingDetailingModule.Controller;
using DrawingDetailingModule.DI;
using DrawingDetailingModule.Interfaces;

namespace DrawingDetailingModule.View
{
    public partial class FormDrawingDetailing : Form
    {
        private IController controller;
        public double FontSize => (double)numFontSizeUpDown.Value;
        // Access interfaces through properties
        //private ISelectionService SelectionService => control.SelectionService;
        //private INXSessionProvider SessionProvider => control.SessionProvider;
        public FormDrawingDetailing(IController controller)
        {
            InitializeComponent();
            this.controller = controller;
            InitNumFontSizeUpDownValue(controller.GetDimensionTextSize);
        }

        private void InitNumFontSizeUpDownValue(Func<int> getDimensionTextSize)
        {
            int fontSize = GetFontSize(getDimensionTextSize);
            numFontSizeUpDown.Value = fontSize;
        }

        private int GetFontSize(Func<int> getDimensionTextSize)
        {
            if (getDimensionTextSize() == 1)
            {
                return 1;
            }            
            return getDimensionTextSize() - 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectFace_Click(object sender, EventArgs e)
        {
            var selectionService = ServiceProvider.SelectionService;
            selectionService.SelectedBody = selectionService.SelectBody();
            updateBtnSelectFaceStage();
            updateBtnApplyStage();
        }

        private void updateBtnApplyStage()
        {
            var selectionService = ServiceProvider.SelectionService;
            btnApply.Enabled =
                selectionService.IsFaceSelected &&
                selectionService.IsPointLocated &&
                FontSize > 0;
        }

        private void updateBtnSelectFaceStage()
        {
            if (ServiceProvider.SelectionService.SelectedBody.Count > 0)
            {
                btnSelectFace.Image = Properties.Resources.correct;
            }
        }

        private void btnPickPoint_Click(object sender, EventArgs e)
        {            
            try
            {
                var selectionService = ServiceProvider.SelectionService;
                selectionService.LocatedPoint = selectionService.SelectScreenPosition();
                updateBtnPickPointStage();
                updateBtnApplyStage();
            }
            catch (Exception err)
            {
                ServiceProvider.SessionProvider.ShowMessageBox(
                    "Error", 
                    NXOpen.NXMessageBox.DialogType.Error, 
                    $"You have accidentaly click the button twice.\n Here is the error message: {err.Message}.");
            }
        }

        private void updateBtnPickPointStage()
        {
            if (ServiceProvider.SelectionService.LocatedPoint.Count > 0)
            {
                btnPickPoint.Image = Properties.Resources.correct;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            controller.Start();
            this.Close();
        }

        private void numFontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            updateBtnApplyStage();
        }
    }
}
