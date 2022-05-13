using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallArea
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберите элементы");
            var elementList = new List<Wall>();

            double area = 0;

            foreach (var selectedElement in selectedElementRefList)
            {
                Wall wall = doc.GetElement(selectedElement) as Wall;
                Parameter areaPar = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
                if (areaPar.StorageType == StorageType.Double)
                {
                    area += areaPar.AsDouble();
                }
            }

            double areaMeters = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

            TaskDialog.Show("Площадь выбранных стен", areaMeters.ToString());

            return Result.Succeeded;
        }
    }
}
