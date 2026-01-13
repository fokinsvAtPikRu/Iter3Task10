using Iter3Task10.Abstraction;
using Iter3Task10.Services;
using Iter3Task10.ViewModels;
using Iter3Task10.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;
using System.Windows;

namespace Iter3Task10
{
    public class Config : ICommandConfiguration
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<RevitTask>(new RevitTask());
            services.AddSingleton<IGetCategoryNamesSevice, GetCategoryNameService>();
            services.AddSingleton<IGetFamilySymbolsService, GetFamilySymbolsService>();
            services.AddSingleton<IGetStartPointService, GetStartPointService>();
            services.AddSingleton<IGetLevelsService, GetLevelsService>();
            services.AddSingleton<IPlaceService, PlaceService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<MainWindow>(provider =>
            {
                var viewModel = provider.GetRequiredService<MainWindowViewModel>();

                var window = ActivatorUtilities.CreateInstance<MainWindow>(provider, viewModel);

                var windowService = provider.GetRequiredService<IWindowService>() as WindowService;
                windowService?.SetWindow(window);

                return window;
            });
        }
    }
}

