using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyNotedUiWpf.Views;
using System.Windows;
using System.Windows.Input;

namespace SimplyNotedUiWpf.ViewModels
{
    public partial class ConfirmViewModel : ObservableObject
    {
        private readonly ConfirmView _confirmView;

        public bool Confirmed { get; private set; }

        public ConfirmViewModel(string title, string message)
        {
            Message = message;
            Title = title;

            _confirmView = new(this);
            _confirmView.ShowDialog();
        }

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        [RelayCommand]
        private void ConfirmViewMoveByHeaderLabelMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                _confirmView.DragMove();
        }

        [RelayCommand]
        private void Yes(Window window)
        {
            Confirmed = true;
            window.Close();
        }

        [RelayCommand]
        private static void No(Window window)
        {
            window.Close();
        }
    }
}