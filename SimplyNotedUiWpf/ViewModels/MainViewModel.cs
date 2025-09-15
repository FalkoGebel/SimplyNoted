using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;
using System.Windows;

namespace SimplyNotedUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly string _pathFile = Application.Current.Properties["PathFile"] as string
                                                ?? throw new ArgumentNullException(nameof(_pathFile));
        private Notes _notes = new();

        public MainViewModel()
        {
            UpdateNotesFromFile();
        }

        [ObservableProperty]
        private List<NoteModel> _noteModels = [];

        [RelayCommand]
        private void NewNote()
        {
            int id = _notes.AddNote();
            NoteModel newNote = _notes.GetNote(id);
            newNote.Title = Properties.Literals.MainViewModel_NewNote_DefaultTitle;
            _notes.UpdateNote(newNote);
            _notes.SaveToFile(_pathFile);
            UpdateNotesFromFile();
        }

        private void UpdateNotesFromFile()
        {
            _notes = Notes.LoadFromFile(_pathFile);
            NoteModels = _notes.CurrentNotes;
        }
    }
}