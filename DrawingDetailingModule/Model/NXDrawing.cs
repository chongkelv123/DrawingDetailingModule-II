using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.UF;
using NXOpenUI;

using DrawingDetailingModule.Controller;
using NXOpen.Features;

namespace DrawingDetailingModule.Model
{
    public class NXDrawing
    {
        Session session;
        Part workPart;
        UI ui;
        UFSession ufs;
        Control control;

        const string THREADED = "Threaded";       
        const string COUNTERBORED = "Counterbored";

        public NXDrawing(Control control)
        {
            session = Session.GetSession();
            ufs = UFSession.GetUFSession();
            workPart = session.Parts.Work;
            ui = UI.GetUI();

            this.control = control;
            //System.Diagnostics.Debugger.Launch();
            var FeatureCollection = workPart.Features;            
            IterateFeatures(FeatureCollection);
        }

        private void SelectFaceMethod()
        {
            Selection selManager = ui.SelectionManager;
            TaggedObject selectedObject;
            Point3d cursor;
            string message = "Please select a face to be measure";
            string title = "Selection";
            var scope = NXOpen.Selection.SelectionScope.WorkPartAndOccurrence;
            var action = NXOpen.Selection.SelectionAction.ClearAndEnableSpecific;
            bool keepHighlighted = false;
            bool includeFeature = false;

            //var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_solid_type, UFConstants.UF_solid_face_subtype, UFConstants.UF_UI_SEL_FEATURE_ANY_FACE);
            //Selection.MaskTriple[] maskArray = new Selection.MaskTriple[] { dimMask };
            //var response = selManager.SelectTaggedObject(message, title, scope, action, includeFeature, keepHighlighted, maskArray, out selectedObject, out cursor);

            Selection.SelectionType[] typeArray = { Selection.SelectionType.Features };

            var response = selManager.SelectTaggedObject(message, title, scope, keepHighlighted, typeArray, out selectedObject, out cursor);
            if (response != NXOpen.Selection.Response.Cancel && response != NXOpen.Selection.Response.Back)
            {
                //System.Diagnostics.Debugger.Launch();           
                int edit = 0;
                string diameter1;
                string diameter2;
                string depth1;
                string depth2;
                string tip_angle;
                int thru_flag;
                ufs.Modl.AskCBoreHoleParms(selectedObject.Tag, edit, out diameter1, out diameter2, out depth1, out depth2, out tip_angle, out thru_flag);
                int num_points = 0;

                ufs.Modl.AskPointsParms(selectedObject.Tag, out num_points, out _);
            }

        }

        private static void IterateFeatures(NXOpen.Features.FeatureCollection FeatureCollection)
        {
            foreach (var item in FeatureCollection)
            {                
                if (item.GetType() == typeof(NXOpen.Features.HolePackage))
                {
                    NXOpen.Features.HolePackage holePackage = item as NXOpen.Features.HolePackage;
                    //TaggedObject taggedObject = NXOpen.Utilities.NXObjectManager.Get(hole.Tag);
                    if (holePackage.GetFeatureName().Contains("Threaded"))
                    {
                        Threaded threaded = new Threaded(holePackage);
                        Guide.InfoWriteLine(threaded.ToString());
                        Guide.InfoWriteLine(threaded.ToString(threaded.GetLocation()));
                    }
                    else if (holePackage.GetFeatureName().Contains("Counterbored"))
                    {
                        Counterbore cb = new Counterbore(holePackage);                        
                        Guide.InfoWriteLine(cb.ToString());
                        Guide.InfoWriteLine(cb.ToString(cb.GetLocation()));
                    }
                    else
                    {
                        SimpleHole hole = new SimpleHole(holePackage);                        
                        Guide.InfoWriteLine(hole.ToString());
                        Guide.InfoWriteLine(hole.ToString(hole.GetLocation()));
                    }
                }
                if (item.GetType() == typeof(NXOpen.Features.Extrude))
                {
                    NXOpen.Features.Extrude extrude = item as NXOpen.Features.Extrude;
                    //Guide.InfoWriteLine($"Extrude feature name: {extrude.GetFeatureName()}");
                }
                if (item.GetType() == typeof(NXOpen.Features.PatternFeature))
                {

                    NXOpen.Features.PatternFeature patternFeature = item as NXOpen.Features.PatternFeature;
                    var childs = patternFeature.GetAllChildren();
                    //Guide.InfoWriteLine($"PatternFeature feature name: {patternFeature.GetFeatureName()}");
                    var points = patternFeature.GetAssociatedCurvesPointsDatums();
                    Part part = Session.GetSession().Parts.Work;
                    NXOpen.Features.PatternFeatureBuilder patternFeatureBuilder = part.Features.CreatePatternFeatureBuilder(patternFeature);
                    var features = patternFeatureBuilder.FeatureList;
                }

            }
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
