using Autodesk.Revit.Attributes;
using Iter3Task10.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Command.Revit;
using RxBim.Shared;
using System;
using System.Windows;

namespace Iter3Task10
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : RxBimCommand
    {
        public PluginResult ExecuteCommand(IServiceProvider provider)
        {
            var mainWindow = provider.GetRequiredService<MainWindow>();           
            mainWindow.Show();
            return PluginResult.Succeeded;
        }
    }
}
