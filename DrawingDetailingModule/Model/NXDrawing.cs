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
            //SelectFaceMethod();
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
                Guide.InfoWriteLine($"Feature name: {selectedObject.ToString()}");
            }

        }

        private static void IterateFeatures(NXOpen.Features.FeatureCollection FeatureCollection)
        {            

            foreach (Feature feature in FeatureCollection)
            {
                if (feature.GetType() == typeof(NXOpen.Features.HolePackage))
                {
                    
                    Part part = Session.GetSession().Parts.Work;
                    AttributeIterator iterator = part.CreateAttributeIterator();
                    iterator.SetIncludeOnlyCategory(FeatureFactory.MACHINING);
                   
                    NXOpen.Features.HolePackage holePackage = feature as NXOpen.Features.HolePackage;
                    if (holePackage.JournalIdentifier.IndexOf(FeatureFactory.THREADED, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        Threaded threaded = new Threaded(holePackage);
                        threaded.GetHoleDetailInformation(holePackage);
                        string result = threaded.ToString();
                        Guide.InfoWriteLine(result);
                        Guide.InfoWriteLine(threaded.ToString(threaded.GetLocation()));
                    }
                    else if (holePackage.JournalIdentifier.IndexOf(FeatureFactory.COUNTERBORED, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (feature.HasUserAttribute(iterator))
                        {
                            string type = feature.GetStringUserAttribute(FeatureFactory.TYPE, 0);
                            switch (type)
                            {
                                case FeatureFactory.REAM:
                                    break;
                                case FeatureFactory.WC:
                                    break;
                                default:
                                    break;
                            }
                            continue;
                        }


                        Counterbore cb = new Counterbore(holePackage);
                        cb.GetHoleDetailInformation(holePackage);
                        string result = cb.ToString();
                        Guide.InfoWriteLine(result);
                        Guide.InfoWriteLine(cb.ToString(cb.GetLocation()));
                    }
                    else
                    {
                        SimpleHole hole = new SimpleHole(holePackage);
                        hole.GetFeatureDetailInformation(holePackage);
                        string result = hole.ToString();
                        Guide.InfoWriteLine(result);
                        Guide.InfoWriteLine(hole.ToString(hole.GetLocation()));
                    }
                }
                if (feature.GetType() == typeof(NXOpen.Features.Extrude))
                {
                    NXOpen.Features.Extrude extrude = feature as NXOpen.Features.Extrude;
                    //Guide.InfoWriteLine($"Extrude feature name: {extrude.GetFeatureName()}");
                }
                if (feature.GetType() == typeof(NXOpen.Features.PatternFeature))
                {

                    NXOpen.Features.PatternFeature patternFeature = feature as NXOpen.Features.PatternFeature;
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
