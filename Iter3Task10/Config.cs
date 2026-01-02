using Iter3Task10.Abstraction;
using Iter3Task10.Services;
using Iter3Task10.ViewModels;
using Iter3Task10.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;

namespace Iter3Task10
{
    public class Config : ICommandConfiguration
    {
        public void Configure(IServiceCollection services)
        {            
            services.AddSingleton<IGetCategoryNamesSevice,GetCategoryNameService>();
            services.AddSingleton<IGetFamilySymbolsService,GetFamilySymbolsService>();
            services.AddSingleton<IGetLevelsService,GetLevelsService>();
            services.AddSingleton<IPlaceService, PlaceService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
        }
    }
}
