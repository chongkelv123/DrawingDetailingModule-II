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
using NXOpen.Annotations;

namespace DrawingDetailingModule.Model
{
    public class NXDrawing
    {
        Session session;
        Part workPart;
        UI ui;
        UFSession ufs;
        Control control;

        HashSet<Point3d> pointOnFaceCollection;
        Tag selectedFaceTag;
        List<TaggedObject> selectedFaces;
        List<Point3d> locatedPoint;
        public List<TaggedObject> SelectedFaces
        {
            get { return selectedFaces; }
            set { selectedFaces = value; }
        }
        public List<Point3d> LocatedPoint
        {
            get { return locatedPoint; }
            set { locatedPoint = value; }
        }
        public bool IsFaceSelected => selectedFaces.Count > 0;
        public bool IsPointLocated => locatedPoint.Count > 0;

        public NXDrawing(Control control)
        {
            session = Session.GetSession();
            ufs = UFSession.GetUFSession();
            workPart = session.Parts.Work;
            ui = UI.GetUI();

            pointOnFaceCollection = new HashSet<Point3d>();
            selectedFaces = new List<TaggedObject>();
            locatedPoint = new List<Point3d>();

            this.control = control;
        }

        public List<TaggedObject> SelectFaces()
        {
            Selection selManager = ui.SelectionManager;
            TaggedObject[] selectedObjects;
            string message = "Please choose a face to begin the Detailing process.";
            string title = "Face Selection";
            var scope = NXOpen.Selection.SelectionScope.WorkPartAndOccurrence;
            var action = NXOpen.Selection.SelectionAction.ClearAndEnableSpecific;
            bool keepHighlighted = false;
            bool includeFeature = false;

            var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_solid_type, UFConstants.UF_solid_face_subtype, UFConstants.UF_UI_SEL_FEATURE_ANY_FACE);
            Selection.MaskTriple[] maskArray = new Selection.MaskTriple[] { dimMask };
            var response = selManager.SelectTaggedObjects(message, title, scope, action, includeFeature, keepHighlighted, maskArray, out selectedObjects);

            if (response == NXOpen.Selection.Response.Cancel && response == NXOpen.Selection.Response.Back)
            {
                return null;
            }
            return selectedObjects.ToList();
        }

        public List<Point3d> SelectScreenPosition()
        {
            Selection selManager = ui.SelectionManager;
            string message = "Indicate the position to place the description table.";
            NXOpen.View theView;
            Point3d pt;

            var resp = selManager.SelectScreenPosition(message, out theView, out pt);
            if (resp != Selection.DialogResponse.Pick)
            {
                return null;
            }
            //Guide.InfoWriteLine($"Point location: ({pt.X:F3}), ({pt.Y:F3}), ({pt.Z:F3})");
            //Guide.InfoWriteLine($"View name: {theView.Name}");
            List<Point3d> result = new List<Point3d>();
            result.Add(pt);
            return result;
        }

        private void SelectFaceMethod()
        {
            Selection selManager = ui.SelectionManager;
            TaggedObject selectedObject;
            Point3d cursor;
            string message = "Please choose a face to begin the Detailing process.";
            string title = "Face Selection";
            var scope = NXOpen.Selection.SelectionScope.WorkPartAndOccurrence;
            var action = NXOpen.Selection.SelectionAction.ClearAndEnableSpecific;
            bool keepHighlighted = false;
            bool includeFeature = false;

            var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_solid_type, UFConstants.UF_solid_face_subtype, UFConstants.UF_UI_SEL_FEATURE_ANY_FACE);
            Selection.MaskTriple[] maskArray = new Selection.MaskTriple[] { dimMask };
            var response = selManager.SelectTaggedObject(message, title, scope, action, includeFeature, keepHighlighted, maskArray, out selectedObject, out cursor);

            if (response == NXOpen.Selection.Response.Cancel && response == NXOpen.Selection.Response.Back)
            {
                //ufs.Modl.AskPointContainment(point, body, out pt_status);
                return;
            }

            selectedFaceTag = selectedObject.Tag;


            IterateFeatures_old();

            NXOpen.Annotations.PmiPreferencesBuilder pmiPreferencesBuilder;
            pmiPreferencesBuilder = workPart.PmiSettingsManager.CreatePreferencesBuilder();
            double currentTextSize = GetCurrentTextSize(pmiPreferencesBuilder);
            SetTextSize(pmiPreferencesBuilder, 10.0);

            //string[] textLines = new string[] { "Test1", "Text2", "Qty: 2" };
            //Point3d origin = new Point3d(200, 50, 0);            

            //AxisOrientation axisOrientation = AxisOrientation.Horizontal;
            //NXOpen.Annotations.Note note = workPart.Annotations.CreateNote(textLines, origin, axisOrientation, null, null);


            TableSection table = CreateTable(new Point3d(500, 600, 0));

            SetTextSize(pmiPreferencesBuilder, currentTextSize);
            pmiPreferencesBuilder.Destroy();
        }

        public TableSection CreateTable(Point3d insertionPoint, List<MachiningDescriptionModel> descriptionModels)
        {
            int numOfColumns = 3, numOfRows = descriptionModels.Count + 1;
            double colWidth = 200.0;
            PmiTableSection nullPmiTableSection = null;
            PmiTableBuilder pmiTableBuilder;
            pmiTableBuilder = workPart.Annotations.PmiTableSections.CreatePmiTableBuilder(nullPmiTableSection);

            pmiTableBuilder.NumberOfColumns = numOfColumns;
            pmiTableBuilder.NumberOfRows = numOfRows;
            pmiTableBuilder.ColumnWidth = colWidth;

            pmiTableBuilder.Origin.OriginPoint = insertionPoint;

            NXObject tableObj = pmiTableBuilder.Commit();

            TableSection table = tableObj as TableSection;

            table.SetName("Machining Table");

            Tag cell = Tag.Null;
            Tag row = Tag.Null;
            Tag column = Tag.Null;
            Tag tabNote = Tag.Null;            

            ufs.Tabnot.AskTabularNoteOfSection(table.Tag, out tabNote);
            ufs.Tabnot.AskNthRow(tabNote, 0, out row);
            ufs.Tabnot.AskNthColumn(tabNote, 0, out column);

            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "HOLE");
            ufs.Tabnot.SetColumnWidth(column, 60);

            ufs.Tabnot.AskNthColumn(tabNote, 1, out column);
            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "DESCRIPTION");
            ufs.Tabnot.SetColumnWidth(column, 200);

            ufs.Tabnot.AskNthColumn(tabNote, 2, out column);
            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "QTY");
            ufs.Tabnot.SetColumnWidth(column, 60);
            
            int numOfColumn = 3;
            for (int i = 0; i < descriptionModels.Count; i++)
            {
                ufs.Tabnot.AskNthRow(tabNote, i+1, out row);
                for (int j = 0; j < numOfColumn; j++)
                {                    
                    ufs.Tabnot.AskNthColumn(tabNote, j, out column);
                    ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
                    if(j == 0)
                    {
                        ufs.Tabnot.SetCellText(cell, NumberToAlphabet(i));
                    }
                    else if(j == 1)
                    {
                        ufs.Tabnot.SetCellText(cell, descriptionModels[i].Description);
                    }
                    else
                    {
                        ufs.Tabnot.SetCellText(cell, descriptionModels[i].Quantity.ToString());
                    }
                }
            }

            pmiTableBuilder.Destroy();
            return table;
        }

        public string NumberToAlphabet(int number)
        {
            int asciiDec = 65;
            char c;
            StringBuilder stringBuilder = new StringBuilder();

            asciiDec += number;
            if(asciiDec == 73)
            {
                asciiDec++;
            }else if (asciiDec == 79)
            {
                asciiDec++;
            }

            if(asciiDec <= 90)
            {
                c = (char)asciiDec;
                stringBuilder.Append(c);
            }
            else
            {
                c = (char)asciiDec;
                stringBuilder.Append(c);
                char d = (char)(number - 26);
                stringBuilder.Append(d);
            }            

            return stringBuilder.ToString();
        }

        public TableSection CreateTable(Point3d insertionPoint)
        {
            int numOfColumns = 3, numOfRows = 8;
            double colWidth = 200.0;
            PmiTableSection nullPmiTableSection = null;
            PmiTableBuilder pmiTableBuilder;
            pmiTableBuilder = workPart.Annotations.PmiTableSections.CreatePmiTableBuilder(nullPmiTableSection);

            pmiTableBuilder.NumberOfColumns = numOfColumns;
            pmiTableBuilder.NumberOfRows = numOfRows;
            pmiTableBuilder.ColumnWidth = colWidth;


            pmiTableBuilder.Origin.OriginPoint = insertionPoint;

            NXObject tableObj = pmiTableBuilder.Commit();

            //System.Diagnostics.Debugger.Launch();
            TableSection table = tableObj as TableSection;



            table.SetName("Machining Table");

            Tag cell = Tag.Null;
            Tag row = Tag.Null;
            Tag column = Tag.Null;
            Tag tabNote = Tag.Null;

            ufs.Tabnot.AskTabularNoteOfSection(table.Tag, out tabNote);
            ufs.Tabnot.AskNthRow(tabNote, 0, out row);
            ufs.Tabnot.AskNthColumn(tabNote, 0, out column);

            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "HOLE");
            ufs.Tabnot.SetColumnWidth(column, 60);

            ufs.Tabnot.AskNthColumn(tabNote, 1, out column);
            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "DESCRIPTION");
            ufs.Tabnot.SetColumnWidth(column, 200);

            ufs.Tabnot.AskNthColumn(tabNote, 2, out column);
            ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
            ufs.Tabnot.SetCellText(cell, "QTY");
            ufs.Tabnot.SetColumnWidth(column, 60);

            pmiTableBuilder.Destroy();
            return table;
        }

        private static void SetTextSize(PmiPreferencesBuilder pmiPreferencesBuilder, double currentTextSize)
        {
            pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize = currentTextSize;
            pmiPreferencesBuilder.Commit();
        }
        public void SetTextSize(double currentTextSize)
        {
            PmiPreferencesBuilder pmiPreferencesBuilder;
            pmiPreferencesBuilder = workPart.PmiSettingsManager.CreatePreferencesBuilder();
            pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize = currentTextSize;
            pmiPreferencesBuilder.Commit();
        }

        private static double GetCurrentTextSize(PmiPreferencesBuilder pmiPreferencesBuilder)
        {
            return pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize;
        }
        public double GetCurrentTextSize()
        {
            PmiPreferencesBuilder pmiPreferencesBuilder;
            pmiPreferencesBuilder = workPart.PmiSettingsManager.CreatePreferencesBuilder();
            return pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize;
        }

        public void IterateFeatures_old()
        {
            var featureCollection = workPart.Features;
            FeatureFactory factory = new FeatureFactory();
            foreach (Feature feature in featureCollection)
            {
                if (feature.GetType() == typeof(NXOpen.Features.HolePackage))
                {
                    NXOpen.Features.HolePackage holePackage = feature as NXOpen.Features.HolePackage;
                    MyFeature feat = factory.GetFeature(feature);
                    feat.GetFeatureDetailInformation(holePackage);
                    string result = feat.ToString();
                    Guide.InfoWriteLine(result);

                    //Guide.InfoWriteLine(feat.ToString(feat.GetLocation()));
                }
                if (feature.GetType() == typeof(NXOpen.Features.Extrude))
                {
                    NXOpen.Features.Extrude extrude = feature as NXOpen.Features.Extrude;
                    //Guide.InfoWriteLine($"The JournalIdentifier: {MyFeature.GetProcessType(feature)}");
                }
                if (feature.GetType() == typeof(NXOpen.Features.PatternFeature))
                {

                    NXOpen.Features.PatternFeature patternFeature = feature as NXOpen.Features.PatternFeature;
                    var childs = patternFeature.GetAllChildren();
                    var points = patternFeature.GetAssociatedCurvesPointsDatums();
                    Part part = Session.GetSession().Parts.Work;
                    NXOpen.Features.PatternFeatureBuilder patternFeatureBuilder = part.Features.CreatePatternFeatureBuilder(patternFeature);
                    var features = patternFeatureBuilder.FeatureList;
                }
            }
        }

        public List<MachiningDescriptionModel> IterateFeatures()
        {
            var featureCollection = workPart.Features;
            FeatureFactory factory = new FeatureFactory();
            
            List<MachiningDescriptionModel> descriptionModels = new List<MachiningDescriptionModel>();

            foreach (Feature feature in featureCollection)
            {
                if (feature.GetType() == typeof(NXOpen.Features.HolePackage))
                {
                    NXOpen.Features.HolePackage holePackage = feature as NXOpen.Features.HolePackage;
                    MyFeature feat = factory.GetFeature(feature);
                    feat.GetFeatureDetailInformation(holePackage);
                    string description = feat.ToString();
                    int qty = feat.Quantity;
                    //MachiningDescriptionModel descModel = new MachiningDescriptionModel();
                    //descModel.Description = description;
                    //descModel.Quantity = qty;
                    descriptionModels.Add(new MachiningDescriptionModel(description, qty));
                }
                else if (feature.GetType() == typeof(NXOpen.Features.Extrude))
                {
                    NXOpen.Features.Extrude extrude = feature as NXOpen.Features.Extrude;
                }
            }

            return descriptionModels;
        }

        public bool IsDrawingOpen()
        {
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
