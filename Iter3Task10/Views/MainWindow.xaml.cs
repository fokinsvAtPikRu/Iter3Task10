using Iter3Task10.ViewModels;
using Serilog;
using System.Windows;

namespace Iter3Task10.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger _logger;
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            _logger=viewModel.Logger;
            InitializeComponent();
        }
        public ILogger Logger { get => _logger; }
    }
}
