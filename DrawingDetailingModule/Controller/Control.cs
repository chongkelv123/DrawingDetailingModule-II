﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;
using DrawingDetailingModule.View;
using DrawingDetailingModule.Interfaces;

namespace DrawingDetailingModule.Controller
{
    public class Control
    {
        NXDrawing drawing;
        FormDrawingDetailing myForm;
        
        // Add a property that exposes the drawing as an ISelectionService
        public ISelectionService SelectionService => drawing;
        // Keep the existing property for backward compatibility during refactoring
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
            List<MachiningDescriptionModel> descriptionModels = new List<MachiningDescriptionModel>();
            try
            {
                descriptionModels = GetDrawing.IterateFeatures();
                GetDrawing.CreateTable(GetDrawing.LocatedPoint[0], descriptionModels);
                GetDrawing.GenerateWCStartPoints(descriptionModels);
            }
            catch (Exception err)
            {
                GetDrawing.ShowMessageBox("Error", NXOpen.NXMessageBox.DialogType.Error, $"Error detail: {err.Message}");
            }
            GetDrawing.SetTextSize(currentTextSize);
        }

        public int GetDimensionTextSize()
        {
            return drawing.GetDimensionTextSize();
        }
    }
}
