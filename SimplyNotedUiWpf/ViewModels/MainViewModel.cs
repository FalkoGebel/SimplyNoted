using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;
using System.Windows;
using System.Windows.Controls;

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
            UpdateNoteSorting(null);
        }

        [ObservableProperty]
        private List<NoteModel> _noteModels = [];

        [ObservableProperty]
        private NoteModel? _selectedNoteModel;

        [ObservableProperty]
        private NoteModel? _currentNoteModel;

        [ObservableProperty]
        private bool _notesSortingCreatedOldestCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingCreatedLatestCheckmarkVisible;

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

        [RelayCommand]
        private void SaveNote()
        {
            if (CurrentNoteModel == null)
            {
                return;
            }

            _notes.UpdateNote(CurrentNoteModel);
            _notes.SaveToFile(_pathFile);
            UpdateNotesFromFile();
        }

        [RelayCommand]
        private void UpdateNoteSorting(object? obj)
        {
            string? header;

            if (obj is not MenuItem menuItem)
                header = Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedLatest;
            else
                header = menuItem.Header.ToString();

            NotesSortingCreatedLatestCheckmarkVisible = false;
            NotesSortingCreatedOldestCheckmarkVisible = false;

            if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedOldest)
            {
                NoteModels = [.. NoteModels.OrderBy(n => n.CreatedAt)];
                NotesSortingCreatedOldestCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedLatest)
            {
                NoteModels = [.. NoteModels.OrderByDescending(n => n.CreatedAt)];
                NotesSortingCreatedLatestCheckmarkVisible = true;
            }

            // TODO - Implement other sorting options
        }

        partial void OnSelectedNoteModelChanged(NoteModel? value)
        {
            if (value == null)
            {
                CurrentNoteModel = null;
                return;
            }

            CurrentNoteModel = new(value.Id)
            {
                Title = value.Title,
                CreatedAt = value.CreatedAt,
                ModifiedAt = value.ModifiedAt,
                Content = value.Content
            };
        }

        private void UpdateNotesFromFile()
        {
            _notes = Notes.LoadFromFile(_pathFile);
            NoteModels = [.. _notes.CurrentNotes.OrderByDescending(n => n.CreatedAt)];
        }
    }
}