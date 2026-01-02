using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using Iter3Task10.Abstraction;
using System;
using System.Collections.Generic;
using static System.Math;

namespace Iter3Task10.Services
{
    public class PlaceService : IPlaceService
    {
        const int Step = 10;
        private Document _document;

        public PlaceService(Document document)
        {
            _document = document;
        }
        public Result Place(FamilySymbol familySymbol, Level level, int count) =>
            ValidateCount(count)
                .Bind(() => ValidateFamilySymbol(familySymbol))
                .Bind(() => ValidateLevel(level))
                .Bind(() => PlaceInstance(familySymbol, level, count));

        private Result ValidateCount(int count) =>
            count < 1 ? Result.Failure("Количество размещаемых элементов меньше 1") : Result.Success();
        private Result ValidateFamilySymbol(FamilySymbol fs) =>
            fs == null ? Result.Failure("FamilySymbol == null") : Result.Success();
        private Result ValidateLevel(Level level) =>
            level == null ? Result.Failure("Level == null") : Result.Success();

        private Result PlaceInstance(FamilySymbol familySymbol, Level level, int count)
        {
            int squareLength =
            Sqrt(count) - (int)Truncate(Sqrt(count)) == 0 ? (int)Truncate(Sqrt(count)) : (int)Truncate(Sqrt(count)) + 1;
            List<XYZ> points = new List<XYZ>();
            for (int i = 0; i < squareLength; i++)
            {
                for (int j = 0; j < squareLength && i * squareLength + j < count; j++)
                {
                    points.Add(new XYZ(i * Step, j * Step, 0));
                }
            }
            try
            {
                using (Transaction transaction = new Transaction(_document, "Размещение элементов"))
                {
                    transaction.Start();
                    if (!familySymbol.IsActive)
                        familySymbol.Activate();
                    foreach (XYZ point in points)
                    {
                        _document.Create.NewFamilyInstance(
                            point,
                            familySymbol,
                            level,
                            Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
            return Result.Success();
        }
    }
}
