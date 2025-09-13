using CommunityToolkit.Mvvm.ComponentModel;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;

namespace SimplyNotedUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Notes _notes = new();

        public MainViewModel()
        {
        }

        [ObservableProperty]
        private List<NoteModel> _noteModels = [];
    }
}