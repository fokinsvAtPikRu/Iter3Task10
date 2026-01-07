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
        private Document _document;
        private ILoggerService _loggerService;
        private IGetStartPointService _getStartPointService;

        public PlaceService(Document document, 
            ILoggerService loggerService,
            IGetStartPointService getStartPointService)
        {
            _document = document;
            _loggerService = loggerService;
            _getStartPointService = getStartPointService;
        }
        public Result Place(string categoryNameSelected, FamilySymbol familySymbol, Level level, int step, int count) =>
                ValidateCategoryName(categoryNameSelected)
                .Bind(() => ValidateFamilySymbol(familySymbol))
                .Bind(() => ValidateCount(count))
                .Bind(() => ValidateLevel(level))
                .Bind(()=> ValidateStep(step))
                .Bind(() => _getStartPointService.GetStartPoint())
                .Bind((xyz) => PlaceInstance(familySymbol, xyz, level, step, count));

        private Result ValidateCategoryName(string categoryNameSelected) =>
            String.IsNullOrEmpty(categoryNameSelected) ? Result.Failure("Не выбрана категория") : Result.Success();
        private Result ValidateCount(int count) =>
            count < 1 ? Result.Failure("Количество размещаемых элементов меньше 1") : Result.Success();
        private Result ValidateFamilySymbol(FamilySymbol fs) =>
            fs == null ? Result.Failure("Не выбрано семейство") : Result.Success();
        private Result ValidateLevel(Level level) =>
            level == null ? Result.Failure("Не выбран уровень") : Result.Success();
        private Result ValidateStep(int step) =>
            step > 0 ? Result.Success() : Result.Failure("Некорректный шаг");
        private Result PlaceInstance(FamilySymbol familySymbol, XYZ startPoint, Level level, int step, int count)
        {
            int squareLength =
            Sqrt(count) - (int)Truncate(Sqrt(count)) == 0 ? (int)Truncate(Sqrt(count)) : (int)Truncate(Sqrt(count)) + 1;
            double stepFeet = UnitUtils.ConvertToInternalUnits(step, DisplayUnitType.DUT_MILLIMETERS);
            List<XYZ> points = new List<XYZ>();
            for (int i = 0; i < squareLength; i++)
            {
                for (int j = 0; j < squareLength && i * squareLength + j < count; j++)
                {
                    points.Add(new XYZ
                        (startPoint.X + i * stepFeet,
                        startPoint.Y + j * stepFeet,
                        0));
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
                _loggerService.LogError(ex, "Error executing PlaceService");
                return Result.Failure(ex.Message);
            }
            return Result.Success();
        }
    }
}
