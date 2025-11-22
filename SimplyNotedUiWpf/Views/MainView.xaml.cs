using SimplyNotedUiWpf.ViewModels;
using System.Windows;

namespace SimplyNotedUiWpf.Views
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}