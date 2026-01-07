using Iter3Task10.Abstraction;
using Iter3Task10.ViewModels;
using System.Windows;

namespace Iter3Task10.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ILoggerService _loggerService;
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            _loggerService=viewModel.LoggerService;
            InitializeComponent();
        }
    }
}
