using System.Windows;
using RobloxScriptExecutor.UI.ViewModels;

namespace RobloxScriptExecutor.UI.Views
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
