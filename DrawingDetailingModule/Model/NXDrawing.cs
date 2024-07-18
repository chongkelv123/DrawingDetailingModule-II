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

        List<TaggedObject> selectedBodys;
        List<Point3d> locatedPoint;
        public List<TaggedObject> SelectedBodys
        {
            get { return selectedBodys; }
            set { selectedBodys = value; }
        }
        public List<Point3d> LocatedPoint
        {
            get { return locatedPoint; }
            set { locatedPoint = value; }
        }
        public bool IsFaceSelected => selectedBodys.Count > 0;
        public bool IsPointLocated => locatedPoint.Count > 0;

        public NXDrawing(Control control)
        {
            session = Session.GetSession();
            ufs = UFSession.GetUFSession();
            workPart = session.Parts.Work;
            ui = UI.GetUI();

            selectedBodys = new List<TaggedObject>();
            locatedPoint = new List<Point3d>();

            this.control = control;
        }

        public List<TaggedObject> SelectBody()
        {
            Selection selManager = ui.SelectionManager;
            TaggedObject selectedObject;
            string message = "Please choose a solid to begin the Detailing process.";
            string title = "Solid Selection";
            var scope = NXOpen.Selection.SelectionScope.WorkPartAndOccurrence;
            var action = NXOpen.Selection.SelectionAction.ClearAndEnableSpecific;
            bool keepHighlighted = false;
            bool includeFeature = false;
            Point3d cursor;

            //var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_solid_type, UFConstants.UF_solid_face_subtype, UFConstants.UF_UI_SEL_FEATURE_ANY_FACE);
            //var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_datum_plane_type, UFConstants.UF_solid_body_subtype, UFConstants.UF_UI_DATUM_PLANE);

            var dimMask = new Selection.MaskTriple(NXOpen.UF.UFConstants.UF_solid_type, UFConstants.UF_solid_body_subtype, UFConstants.UF_UI_SEL_FEATURE_BODY);
            Selection.MaskTriple[] maskArray = new Selection.MaskTriple[] { dimMask };
            var response = selManager.SelectTaggedObject(message, title, scope, action, includeFeature, keepHighlighted, maskArray, out selectedObject, out cursor);

            if (response == NXOpen.Selection.Response.Cancel && response == NXOpen.Selection.Response.Back)
            {
                return null;
            }

            List<TaggedObject> result = new List<TaggedObject>();
            result.Add(selectedObject);
            return result;
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

            List<Point3d> result = new List<Point3d>();
            result.Add(pt);
            return result;
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
                ufs.Tabnot.AskNthRow(tabNote, i + 1, out row);
                for (int j = 0; j < numOfColumn; j++)
                {
                    ufs.Tabnot.AskNthColumn(tabNote, j, out column);
                    ufs.Tabnot.AskCellAtRowCol(row, column, out cell);
                    if (j == 0)
                    {
                        ufs.Tabnot.SetCellText(cell, NumberToAlphabet(i));
                    }
                    else if (j == 1)
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
            if (asciiDec == 73)
            {
                asciiDec++;
            }
            else if (asciiDec == 79)
            {
                asciiDec++;
            }

            if (asciiDec <= 90)
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

        public void SetTextSize(double currentTextSize)
        {
            PmiPreferencesBuilder pmiPreferencesBuilder;
            pmiPreferencesBuilder = workPart.PmiSettingsManager.CreatePreferencesBuilder();
            pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize = currentTextSize;
            pmiPreferencesBuilder.Commit();
        }

        public double GetCurrentTextSize()
        {
            PmiPreferencesBuilder pmiPreferencesBuilder;
            pmiPreferencesBuilder = workPart.PmiSettingsManager.CreatePreferencesBuilder();
            return pmiPreferencesBuilder.AnnotationStyle.LetteringStyle.GeneralTextSize;
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

                    List<Point3d> points = feat.GetLocation();
                    List<Point3d> outPoints = new List<Point3d>();

                    if (IsPointContainInBoundingBox(points, selectedBodys[0].Tag, out outPoints))
                    {
                        descriptionModels.Add(new MachiningDescriptionModel(description, outPoints.Count, outPoints));
                    }
                }
                else if (feature.GetType() == typeof(NXOpen.Features.Extrude))
                {
                    NXOpen.Features.Extrude extrude = feature as NXOpen.Features.Extrude;
                }
            }

            return descriptionModels;
        }

        private NXObject CreateBoundingBox(TaggedObject bodyTagObject)
        {
            NXOpen.Features.ToolingBoxBuilder toolingBoxBuilder;
            toolingBoxBuilder = workPart.Features.ToolingFeatureCollection.CreateToolingBoxBuilder(null);

            toolingBoxBuilder.Type = ToolingBoxBuilder.Types.BoundedBlock;
            NXObject[] selections = new NXObject[1];
            NXObject[] deselections = new NXObject[0];
            selections[0] = bodyTagObject as NXObject;

            System.Diagnostics.Debugger.Launch();
            Body body = bodyTagObject as Body;
            //Point3d location = extrude.Location;            

            Matrix3x3 matrix = new Matrix3x3();
            matrix.Xx = 1.0;
            matrix.Xy = 0.0;
            matrix.Xz = 0.0;
            matrix.Yx = 0.0;
            matrix.Yy = 1.0;
            matrix.Yz = 0.0;
            matrix.Zx = 0.0;
            matrix.Zy = 0.0;
            matrix.Zz = 1.0;
            Point3d position = new Point3d(0.0, 0.0, 0.0);
            toolingBoxBuilder.SetBoxMatrixAndPosition(matrix, position);

            toolingBoxBuilder.SetSelectedOccurrences(selections, deselections);
            toolingBoxBuilder.CalculateBoxSize();

            NXObject boundingBoxObj = toolingBoxBuilder.Commit();
            toolingBoxBuilder.Destroy();

            return boundingBoxObj;
        }



        private bool IsPointContainInBoundingBox(List<Point3d> points, Tag selectedFaceTag, out List<Point3d> outPoints)
        {
            const int INSIDE_BODY = 1;
            const int OUTSIDE_BODY = 2;
            const int ON_BODY = 3;

            bool result = false;
            List<Point3d> pointCollection = new List<Point3d>();
            
            int pt_status = 0;
            AskBoundingBox boundingBox = new AskBoundingBox(ufs, SelectedBodys[0].Tag);
            NXObject boundingBoxObj = boundingBox.CreateBoundingBox();
            Block block = boundingBoxObj as Block;
            Body[] bodies = block.GetBodies();            

            foreach (Point3d p in points)
            {
                double[] pt = new double[] { p.X, p.Y, p.Z };
                
                ufs.Modl.AskPointContainment(pt, bodies[0].Tag, out pt_status);

                switch (pt_status)
                {
                    case OUTSIDE_BODY:
                        continue;
                    case INSIDE_BODY:
                        continue;
                    case ON_BODY:
                        pointCollection.Add(p);
                        result = true;
                        break;
                    default:
                        break;
                }
            }

            outPoints = boundingBox.VerifyPoints(pointCollection);
            Tag[] block_Tags = new Tag[] { block.Tag };
            ufs.Modl.DeleteFeature(block_Tags);

            return result;
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
