using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingDetailingModule.Controller;

namespace DrawingDetailingModule.View
{
    public partial class FormDrawingDetailing : Form
    {
        Controller.Control control;
        public double FontSize => (double)numFontSizeUpDown.Value;
        public FormDrawingDetailing(Controller.Control control)
        {
            InitializeComponent();
            this.control = control;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectFace_Click(object sender, EventArgs e)
        {
            control.GetDrawing.SelectedFaces = control.GetDrawing.SelectFaces();
            updateBtnSelectFaceStage();
            updateBtnApplyStage();
        }

        private void updateBtnApplyStage()
        {
            btnApply.Enabled =
                control.GetDrawing.IsFaceSelected &&
                control.GetDrawing.IsPointLocated &&
                FontSize > 0;
        }

        private void updateBtnSelectFaceStage()
        {
            if (control.GetDrawing.SelectedFaces.Count > 0)
            {
                btnSelectFace.Image = Properties.Resources.correct;
            }
        }

        private void btnPickPoint_Click(object sender, EventArgs e)
        {
            control.GetDrawing.LocatedPoint = control.GetDrawing.SelectScreenPosition();
            updateBtnPickPointStage();
            updateBtnApplyStage();
        }

        private void updateBtnPickPointStage()
        {
            if (control.GetDrawing.SelectedFaces.Count > 0)
            {
                btnPickPoint.Image = Properties.Resources.correct;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            control.Start();
            this.Close();
        }

        private void numFontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            updateBtnApplyStage();
        }
    }
}
