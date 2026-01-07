using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Iter3Task10.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Iter3Task10.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        // RevitTask
        private RevitTask _revitTask;
        // Services
        private readonly ILoggerService _loggerService;
        private readonly IGetCategoryNamesSevice _getCategoryNamesService;
        private readonly IGetFamilySymbolsService _getFamilySymbolsService;
        private readonly IGetLevelsService _getLevelService;
        private readonly IPlaceService _placeService;
        
        // INotifyPropertychanged
        private List<string> _categoryNames;
        private string _categoryNameSelected;
        private List<FamilySymbol> _familySymbols;
        private FamilySymbol _selectedFamilySymbol;
        private int _count = 15;
        private List<Level> _levels;
        private Level _selectedLevel;
        private string _statusMessage;
        private int _step = 1000;

        // Command
        private readonly AsyncRelayCommand _placeItemCommand;
        public MainWindowViewModel(RevitTask revitTask,
            ILoggerService logger,
            IPlaceService placeService,
            IGetCategoryNamesSevice getCategoryTypesSevice,
            IGetFamilySymbolsService getFamilySymbolsService,
            IGetLevelsService getLevelsService)
        {
            _revitTask = revitTask;
            _loggerService = logger;
            _placeItemCommand = new AsyncRelayCommand(PlaceItem);
            _placeService = placeService;
            _getCategoryNamesService = getCategoryTypesSevice;
            _getFamilySymbolsService = getFamilySymbolsService;
            _getLevelService = getLevelsService;
            _categoryNames = _getCategoryNamesService.GetCategoryNames();
            _levels = _getLevelService.GetLevel();            
        }
        // Property for Logger
        public ILoggerService LoggerService { get; }
        
        // Properties for INotifyPropertyChanged
        
        public List<string> CategoryNames
        {
            get => _categoryNames;
            set => SetProperty(ref _categoryNames, value);
        }

        public string CategoryNameSelected
        {
            get => _categoryNameSelected;
            set
            {
                SetProperty(ref _categoryNameSelected, value);
                if (value == null)
                    FamilySymbols?.Clear();
                else
                    FamilySymbols = _getFamilySymbolsService.GetFamilySymbols(value);
            }
        }

        public List<FamilySymbol> FamilySymbols
        {
            get => _familySymbols;
            set => SetProperty(ref _familySymbols, value);
        }

        public FamilySymbol SelectedFamilySymbol
        {
            get => _selectedFamilySymbol;
            set => SetProperty(ref _selectedFamilySymbol, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public List<Level> Levels
        {
            get => _levels;
            set => SetProperty(ref _levels, value);
        }
        public Level SelectedLevel
        {
            get => _selectedLevel;
            set => SetProperty(ref _selectedLevel, value);
        }

        public int Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public AsyncRelayCommand PlaceItemCommand
        {
            get => _placeItemCommand;
        }
        // Method Execute for RelayCommand
        private async Task PlaceItem()
        {
            _loggerService.LogRevitOperation($"Create CreateFamilySymbols {SelectedFamilySymbol.Name} on level {SelectedLevel} step {Step} count {Count}");
            CSharpFunctionalExtensions.Result result = await _revitTask.Run<CSharpFunctionalExtensions.Result>(app =>
                  _placeService.Place(CategoryNameSelected, SelectedFamilySymbol, SelectedLevel, Step, Count));
            if (result.IsSuccess)
            {
                _loggerService.LogInformation("FamilySymbols created");              
                StatusMessage = $"Размещено {Count} экземпляров";
                // TaskDialog.Show("Размещение мебели", $"Размещено {Count} экземпляров");
            }
            else
            {
                _loggerService.LogWarning("Failed to create FamilySymbols");
                StatusMessage = $"Ошибка {result.Error}";
                // TaskDialog.Show("Размещение мебели", $"{result.Error}");
            }
        }
    }
}
