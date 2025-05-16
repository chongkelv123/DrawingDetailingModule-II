﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingDetailingModule.Controller;
using DrawingDetailingModule.Interfaces;

namespace DrawingDetailingModule.View
{
    public partial class FormDrawingDetailing : Form
    {
        Controller.Control control;
        public double FontSize => (double)numFontSizeUpDown.Value;
        // Add a property to access the selection service
        private ISelectionService SelectionService => control.SelectionService;
        public FormDrawingDetailing(Controller.Control control)
        {
            InitializeComponent();
            this.control = control;
            InitNumFontSizeUpDownValue(control.GetDimensionTextSize);
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
            control.GetDrawing.SelectedBody = control.GetDrawing.SelectBody();
            updateBtnSelectFaceStage();
            updateBtnApplyStage();
        }

        private void updateBtnApplyStage()
        {
            btnApply.Enabled =
                SelectionService.IsFaceSelected &&
                SelectionService.IsPointLocated &&
                FontSize > 0;
        }

        private void updateBtnSelectFaceStage()
        {
            if (SelectionService.SelectedBody.Count > 0)
            {
                btnSelectFace.Image = Properties.Resources.correct;
            }
        }

        private void btnPickPoint_Click(object sender, EventArgs e)
        {            
            try
            {
                control.GetDrawing.LocatedPoint = control.GetDrawing.SelectScreenPosition();
                updateBtnPickPointStage();
                updateBtnApplyStage();
            }
            catch (Exception err)
            {
                control.GetDrawing.ShowMessageBox(
                    "Error", 
                    NXOpen.NXMessageBox.DialogType.Error, 
                    $"You have accidentaly click the button twice.\n Here is the error message: {err.Message}.");
            }
        }

        private void updateBtnPickPointStage()
        {
            if (SelectionService.LocatedPoint.Count > 0)
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
