using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using Iter3Task10.Abstraction;
using Iter3Task10.Services;
using System;
using System.Collections.Generic;

namespace Iter3Task10.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {   
        // Services
        private readonly IGetCategoryNamesSevice _getCategoryNamesService;
        private readonly IGetFamilySymbolsService _getFamilySymbolsService;
        private readonly IGetLevelsService _getLevelService;
        private readonly IPlaceService _placeService;
        // INotifyPropertychanged
        private List<string> _categoryNames;
        private string _categoryNameSelected;
        private List<FamilySymbol> _familySymbols;
        private FamilySymbol _selectedFamilySymbol;
        private int _count;
        private List<Level> _levels;
        private Level _selectedLevel;
        private string _statusMessage;
        // ctor
        public MainWindowViewModel(IPlaceService placeService, 
            IGetCategoryNamesSevice getCategoryTypesSevice, 
            IGetFamilySymbolsService getFamilySymbolsService,
            IGetLevelsService getLevelsService)
        {
            PlaceItemCommand = new RelayCommand(PlaceItem);
            _placeService = placeService;
            _getCategoryNamesService = getCategoryTypesSevice;
            _getFamilySymbolsService = getFamilySymbolsService;
            _getLevelService = getLevelsService;
            _categoryNames = _getCategoryNamesService.GetCategoryNames();
            _levels = _getLevelService.GetLevel();
        }
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
                    FamilySymbols= _getFamilySymbolsService.GetFamilySymbols(value);
            }
        }

        public List<FamilySymbol> FamilySymbols 
        { 
            get => _familySymbols; 
            set => SetProperty(ref _familySymbols,value);        
        }

        public FamilySymbol SelectedFamilySymbol
        {
            get => _selectedFamilySymbol;
            set => SetProperty(ref _selectedFamilySymbol, value);
        }

        public int Count 
        { 
            get => _count; 
            set => SetProperty(ref _count , value); 
        }

        public List<Level> Levels
        {
            get => _levels;
            set => SetProperty(ref _levels, value);
        }
        public Level SelectedLevel
        {
            get => _selectedLevel;
            set => SetProperty (ref _selectedLevel, value);
        }

        public string StatusMessage 
        { 
            get => _statusMessage; 
            set => SetProperty(ref _statusMessage , value); 
        }

        public RelayCommand PlaceItemCommand { get; }
        // Method Execute for RelayCommand
        private void PlaceItem()
        {
            CSharpFunctionalExtensions.Result result = _placeService.Place(SelectedFamilySymbol, SelectedLevel,Count);
            if (result.IsSuccess)
            {
                StatusMessage = $"Размещено {Count} экземпляров";
                TaskDialog.Show("Размещение мебели", $"Размещено {Count} экземпляров");
            }
            else
            {
                StatusMessage = $"Ошибка {result.Error}";
                TaskDialog.Show("Размещение мебели", $"{result.Error}");
            }
        }
    }
}
