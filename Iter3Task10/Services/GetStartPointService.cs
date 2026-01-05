using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Iter3Task10.Abstraction;

namespace Iter3Task10.Services
{
    public class GetStartPointService : IGetStartPointService
    {
        private UIDocument _uiDocument;
        private IWindowService _windowService;
        public GetStartPointService(UIDocument uiDocument, IWindowService windowService)
        {            
            _uiDocument = uiDocument;
            _windowService=windowService;
        }
        public CSharpFunctionalExtensions.Result<XYZ> GetStartPoint()
        {
            _windowService.HideWindow();
            try
            {
                XYZ selectedPoint = _uiDocument.Selection.PickPoint("Выберите стартовую точку");
                _windowService.ShowWindow();
                _windowService.BringToFront();
                return CSharpFunctionalExtensions.Result.Success(selectedPoint);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                _windowService.ShowWindow();
                _windowService.BringToFront();
                return CSharpFunctionalExtensions.Result.Failure<XYZ>("Начальная точка не выбрана");
            }            
        }
    }
}
