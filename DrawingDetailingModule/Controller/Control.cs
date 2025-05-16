using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;
using DrawingDetailingModule.View;
using DrawingDetailingModule.Interfaces;
using DrawingDetailingModule.DI;

namespace DrawingDetailingModule.Controller
{
    public class Control: IController
    {        
        FormDrawingDetailing myForm;        
        public FormDrawingDetailing GetForm => myForm;

        public void Initialize()
        {
            if (!ServiceProvider.SessionProvider.IsDrawingOpen())
            {
                return;
            }

            myForm = new FormDrawingDetailing(this);
            myForm.Show();
        }

        public void Start()
        {            
            double currentTextSize = ServiceProvider.SessionProvider.GetCurrentTextSize();
            ServiceProvider.SessionProvider.SetTextSize(myForm.FontSize);
            List<MachiningDescriptionModel> descriptionModels = new List<MachiningDescriptionModel>();
            try
            {
                // Use the interface instead of direct class reference                
                descriptionModels = ServiceProvider.FeatureProcessor.IterateFeatures();
                ServiceProvider.TableService.CreateTable(ServiceProvider.SelectionService.LocatedPoint[0], descriptionModels);
                ServiceProvider.FeatureProcessor.GenerateWCStartPoints(descriptionModels);
            }
            catch (Exception err)
            {
                ServiceProvider.SessionProvider.ShowMessageBox("Error", NXOpen.NXMessageBox.DialogType.Error, $"Error detail: {err.Message}");
            }
            ServiceProvider.SessionProvider.SetTextSize(currentTextSize);
        }

        public int GetDimensionTextSize()
        {
            return ServiceProvider.SessionProvider.GetDimensionTextSize();
        }
    }
}
