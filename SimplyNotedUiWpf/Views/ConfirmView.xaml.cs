using SimplyNotedUiWpf.ViewModels;
using System.Windows;

namespace SimplyNotedUiWpf.Views
{
    /// <summary>
    /// Interaktionslogik für ConfirmView.xaml
    /// </summary>
    public partial class ConfirmView : Window
    {
        public ConfirmView(ConfirmViewModel confirmViewModel)
        {
            InitializeComponent();
            DataContext = confirmViewModel;
        }
    }
}